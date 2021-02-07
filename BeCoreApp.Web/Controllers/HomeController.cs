using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeCoreApp.Models;
using BeCoreApp.Application.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using BeCoreApp.Models.ProjectViewModels;

namespace BeCoreApp.Controllers
{
    public class HomeController : Controller
    {
        IConfiguration _configuration;
        IBlogService _blogService;
        IBlogCategoryService _blogCategoryService;
        IBlockChainService _blockChainService;
        public HomeController(
            IConfiguration configuration,
            IBlogService blogService,
            IBlockChainService blockChainService,
        IBlogCategoryService blogCategoryService)
        {
            _blockChainService = blockChainService;
            _blogService = blogService;
            _blogCategoryService = blogCategoryService;
            _configuration = configuration;
        }

        //[ResponseCache(CacheProfileName ="Default")]
        public IActionResult Index()
        {
            //_blockChainService.CreateEthereumAccount();
            HomeViewModel model = new HomeViewModel();
            model.HomeBlogs = _blogService.GetHomeBlogs();
            model.MainItems = _blogCategoryService.GetMainItems();
            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
