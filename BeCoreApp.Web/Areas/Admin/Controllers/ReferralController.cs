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
    public class ReferralController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IBlogService _blogService;
        private readonly IReferralService _referralService;
        private readonly UserManager<AppUser> _userManager;
        public ReferralController(
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
        public IActionResult Index()
        {
            var userId = User.GetSpecificClaim("UserId");
            var referralGroups = _referralService.GetAll(userId);
            return View(referralGroups);
        }

        [HttpGet]
        public async Task<IActionResult> GetReferralLinkUpline(int blogId)
        {
            var userId = User.GetSpecificClaim("UserId");
            var result = await _referralService.GetReferralLinkUpline(userId, blogId);
            return new OkObjectResult(result);
        }
    }
}