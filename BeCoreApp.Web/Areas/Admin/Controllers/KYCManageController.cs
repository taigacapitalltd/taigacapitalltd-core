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
using BeCoreApp.Authorization;
using BeCoreApp.Data.Enums;
using BeCoreApp.Extensions;
using BeCoreApp.Utilities.Constants;
using BeCoreApp.Utilities.Dtos;
using BeCoreApp.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
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
    public class KYCManageController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IKYCService _kycService;
        private readonly IAuthorizationService _authorizationService;
        public KYCManageController(IUserService userService, IKYCService kycService, IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            _userService = userService;
            _kycService = kycService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "KYC_MANAGE", Operations.Read);
            if (result.Succeeded == false)
                return new RedirectResult("/Admin/Account/Login");

            return View();
        }


        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _kycService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _kycService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> Approval(KYCViewModel kycVm)
        {
            try
            {
                await _kycService.ApprovalAsync(kycVm);
                return new OkObjectResult(new GenericResult(true));
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }

        [HttpPost]
        public IActionResult Reject(KYCViewModel kycVm)
        {
            try
            {
                _kycService.Reject(kycVm);
                return new OkObjectResult(new GenericResult(true));
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }

        [HttpPost]
        public IActionResult Lock(KYCViewModel kycVm)
        {
            try
            {
                _kycService.Lock(kycVm);
                return new OkObjectResult(new GenericResult(true));
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }

        [HttpPost]
        public IActionResult UnLock(KYCViewModel kycVm)
        {
            try
            {
                _kycService.UnLock(kycVm);
                return new OkObjectResult(new GenericResult(true));
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _kycService.Delete(id);
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