using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.BlockChain;
using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Data.Enums;
using BeCoreApp.Extensions;
using BeCoreApp.Models;
using BeCoreApp.Utilities.Constants;
using BeCoreApp.Utilities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BeCoreApp.Areas.Admin.Controllers
{
    public class EcosystemController : BaseController
    {
        private readonly IBlogCategoryService _blogCategoryService;
        public EcosystemController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.MainItems = _blogCategoryService.GetMainItems();
            return View(model);
        }
    }
}