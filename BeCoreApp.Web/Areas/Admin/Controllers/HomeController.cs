using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Data.Enums;
using BeCoreApp.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeCoreApp.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly INotifyService _notifyService;
        public HomeController(INotifyService notifyService)
        {
            _notifyService = notifyService;
        }

        public IActionResult Index()
        {
            var model = _notifyService.GetbyActive();
            return View(model);
        }
    }
}