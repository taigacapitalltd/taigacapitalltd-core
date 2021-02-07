using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.Blog;
using BeCoreApp.Application.ViewModels.Common;
using BeCoreApp.Application.ViewModels.Enterprise;
using BeCoreApp.Application.ViewModels.Product;
using BeCoreApp.Application.ViewModels.Project;
using BeCoreApp.Application.ViewModels.Valuesshare;
using BeCoreApp.Data.Enums;
using BeCoreApp.Extensions;
using BeCoreApp.Utilities.Constants;
using BeCoreApp.Utilities.Dtos;
using BeCoreApp.Utilities.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nethereum.Util;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace BeCoreApp.Areas.Admin.Controllers
{
    public class KYCController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IKYCService _kycService;
        private readonly AddressUtil _addressUtil = new AddressUtil();
        public KYCController(IUserService userService, IKYCService kycService)
        {
            _userService = userService;
            _kycService = kycService;
        }

        public IActionResult Index()
        {
            var userId = User.GetSpecificClaim("UserId");
            bool result = _kycService.CheckShowFormRequestKYC(userId);
            return View(result);
        }

        [HttpGet]
        public IActionResult GetAllByUserId()
        {
            var userId = User.GetSpecificClaim("UserId");
            var model = _kycService.GetAllByUserId(userId);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _kycService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(KYCViewModel kycVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                try
                {
                    if (!_addressUtil.IsChecksumAddress(kycVm.WithdrawPublishKey) || !_addressUtil.IsValidAddressLength(kycVm.WithdrawPublishKey))
                        return new OkObjectResult(new GenericResult(false, "Địa chỉ ví sai định đạng."));

                    var userId = User.GetSpecificClaim("UserId");
                    bool result = _kycService.CheckShowFormRequestKYC(userId);
                    if (result == false)
                        return new OkObjectResult(new GenericResult(false, "KYC của bạn đã được duyệt hoặc đang chờ duyệt, vui lòng liên hệ Admin."));

                    kycVm.AppUserId = new Guid(userId);

                    await _kycService.AddAsync(kycVm);
                    _kycService.Save();
                    return new OkObjectResult(new GenericResult(true));
                }
                catch (Exception ex)
                {
                    return new OkObjectResult(new GenericResult(false, ex.Message));
                }
            }
        }
    }
}