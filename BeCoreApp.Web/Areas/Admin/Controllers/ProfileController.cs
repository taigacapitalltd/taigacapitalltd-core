using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Application.ViewModels.Valuesshare;
using BeCoreApp.Data.Entities;
using BeCoreApp.Extensions;
using BeCoreApp.Models.AccountViewModels;
using BeCoreApp.Utilities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace BeCoreApp.Areas.Admin.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IBlogService _blogService;
        private readonly IReferralService _referralService;
        private readonly UserManager<AppUser> _userManager;
        public ProfileController(
            UserManager<AppUser> userManager,
            IBlogService blogService,
            IUserService userService,
            IReferralService referralService)
        {
            _blogService = blogService;
            _referralService = referralService;
            _userManager = userManager;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.GetSpecificClaim("UserId");

            var model = await _userService.GetById(userId);

            if (model != null)
            {
                var customerReferal = await _userManager.FindByIdAsync(model.ReferralId.ToString());
                if (customerReferal != null)
                {
                    model.Referal = customerReferal.UserName;
                }
            }

            var referralGroups = _referralService.GetAll(userId);
            ViewBag.ReferralGroups = referralGroups;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AppUserViewModel profileVm)
        {
            try
            {
                var userId = User.GetSpecificClaim("UserId");
                var appUser = await _userManager.FindByIdAsync(userId);
                if (appUser == null)
                    return new OkObjectResult(new GenericResult(false, "Tài Khoản không tồn tại"));

                if (appUser.IsUpdatedKYC == true)
                {
                    return new OkObjectResult(new GenericResult(false, "Tài Khoản đã được duyệt kyc"));
                }

                var userCMNDs = _userManager.Users
                    .Where(x => x.CMND == profileVm.CMND && x.Id != appUser.Id);
                if (userCMNDs.Count() > 0)
                    return new OkObjectResult(new GenericResult(false, "Số Điện Thoại đã tồn tại"));

                var userPhones = _userManager.Users
                    .Where(x => x.PhoneNumber == profileVm.PhoneNumber && x.Id != appUser.Id);
                if (userPhones.Count() > 0)
                    return new OkObjectResult(new GenericResult(false, "Số Điện Thoại đã tồn tại"));

                appUser.FullName = profileVm.FullName;
                appUser.PhoneNumber = profileVm.PhoneNumber;
                appUser.CMND = profileVm.CMND;

                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                    return new OkObjectResult(new GenericResult(true, "Tài Khoản đã cập nhật thành công"));
                else
                    return new OkObjectResult(new GenericResult(false, "Tài Khoản cập nhật thất bại"));
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateReferral(ReferralViewModel referralVm)
        {
            try
            {
                var blog = _blogService.GetById(referralVm.BlogId);
                if (referralVm.ReferralLink.Contains(blog.ReferralLinkRule))
                {
                    var userId = User.GetSpecificClaim("UserId");
                    referralVm.AppUserId = new Guid(userId);
                    _referralService.AddOrUpdate(referralVm);
                    _referralService.Save();
                    return new OkObjectResult(new GenericResult(true, "Cập nhật liên kết thành công."));
                }
                else
                {
                    return new OkObjectResult(new GenericResult(false, "Liên kết không đúng quy định. Xin vui lòng xem lại liên kết mẫu."));
                }
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }
    }
}