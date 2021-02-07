using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace BeCoreApp.Areas.Admin.Controllers
{
    public class AppUserBonusController : BaseController
    {
        public IUserBonusService _userBonusService;

        public AppUserBonusController(IUserBonusService userBonusService)
        {
            _userBonusService = userBonusService;
        }

        public IActionResult Index()
        {
            var roleName = User.GetSpecificClaim("RoleName");
            if (roleName.ToLower() != "admin")
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _userBonusService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _userBonusService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(string modelJson)
        {
            var roleName = User.GetSpecificClaim("RoleName");
            if (roleName.ToLower() != "admin")
                return RedirectToAction("Index", "Home");

            UserBonusViewModel appUserBonusVm = JsonConvert.DeserializeObject<UserBonusViewModel>(modelJson);
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            _userBonusService.Update(appUserBonusVm);
            _userBonusService.Save();

            return new OkObjectResult(appUserBonusVm);
        }
    }
}