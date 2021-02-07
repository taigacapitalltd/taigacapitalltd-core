using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.Enterprise;
using BeCoreApp.Application.ViewModels.Product;
using BeCoreApp.Application.ViewModels.Project;
using BeCoreApp.Utilities.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace BeCoreApp.Areas.Admin.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IEnterpriseService _enterpriseService;
        private readonly IFieldService _fieldService;
        private readonly IEnterpriseFieldService _enterpriseFieldService;
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;
        private readonly IWardService _wardService;
        private readonly IProjectService _projectService;
        private readonly IProjectCategoryService _projectCategoryService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ProjectController(
            IEnterpriseService enterpriseService,
            IFieldService fieldService,
            IProvinceService provinceService,
            IDistrictService districtService,
            IEnterpriseFieldService enterpriseFieldService,
            IWardService wardService,
            IProjectCategoryService projectCategoryService,
            IProjectService projectService,
            IHostingEnvironment hostingEnvironment)
        {
            _enterpriseFieldService = enterpriseFieldService;
            _enterpriseService = enterpriseService;
            _fieldService = fieldService;
            _provinceService = provinceService;
            _districtService = districtService;
            _wardService = wardService;
            _projectCategoryService = projectCategoryService;
            _projectService = projectService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var enterprises = _enterpriseService.GetAll();
            ViewBag.EnterpriseId = new SelectList(enterprises, "Id", "Name");

            var projectCategories = _projectCategoryService.GetAll();
            ViewBag.ProjectCategoryId = new SelectList(projectCategories, "Id", "Name");

            var provinces = _provinceService.GetAll();
            ViewBag.ProvinceId = new SelectList(provinces, "Id", "Name");

            var districts = _districtService.GetAll();
            ViewBag.DistrictId = new SelectList(districts, "Id", "Name");

            var wards = _wardService.GetAll();
            ViewBag.WardId = new SelectList(wards, "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult SaveImages(int projectId, string[] images)
        {
            _projectService.AddImages(projectId, images);
            _projectService.Save();
            return new OkObjectResult(images);
        }

        [HttpGet]
        public IActionResult GetImages(int projectId)
        {
            var images = _projectService.GetImages(projectId);
            return new OkObjectResult(images);
        }

        [HttpPost]
        public IActionResult SaveLibraries(int projectId, string[] images)
        {
            _projectService.AddLibraries(projectId, images);
            _projectService.Save();
            return new OkObjectResult(images);
        }

        [HttpGet]
        public IActionResult GetLibraries(int projectId)
        {
            var images = _projectService.GetLibraries(projectId);
            return new OkObjectResult(images);
        }

        public IActionResult GetDistricts(int provinceId, bool type)
        {
            var districts = _districtService.GetAllByProvinceId(provinceId);
            ViewBag.DistrictId = new SelectList(districts, "Id", "Name");
            return PartialView(type);
        }

        public IActionResult GetWards(int districtId, bool type)
        {
            var wards = _wardService.GetAllByDistrictId(districtId);
            ViewBag.WardId = new SelectList(wards, "Id", "Name");
            return PartialView(type);
        }


        #region AJAX API

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int provinceId,
            int districtId, int wardId, int enterpriseId,
            int projectCategoryId, int page, int pageSize)
        {
            var model = _projectService.GetAllPaging("", "",
                keyword, provinceId,
                districtId, wardId,
                enterpriseId, projectCategoryId,
                page, pageSize);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _projectService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(ProjectViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (vm.Id == 0)
                    _projectService.Add(vm);
                else
                    _projectService.Update(vm);

                _projectService.Save();
                return new OkObjectResult(vm);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);
            else
            {
                _projectService.Delete(id);
                _projectService.Save();

                return new OkObjectResult(id);
            }
        }

        #endregion
    }
}