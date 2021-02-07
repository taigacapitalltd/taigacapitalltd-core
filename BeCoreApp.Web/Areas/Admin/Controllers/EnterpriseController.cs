using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.Enterprise;
using BeCoreApp.Application.ViewModels.Product;
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
    public class EnterpriseController : BaseController
    {
        private readonly IEnterpriseService _enterpriseService;
        private readonly IFieldService _fieldService;
        private readonly IEnterpriseFieldService _enterpriseFieldService;
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;
        private readonly IWardService _wardService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public EnterpriseController(IEnterpriseService enterpriseService,
            IFieldService fieldService,
            IProvinceService provinceService, IDistrictService districtService,
            IEnterpriseFieldService enterpriseFieldService,
            IWardService wardService, IHostingEnvironment hostingEnvironment)
        {
            _enterpriseFieldService = enterpriseFieldService;
            _enterpriseService = enterpriseService;
            _fieldService = fieldService;
            _provinceService = provinceService;
            _districtService = districtService;
            _wardService = wardService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var fields = _fieldService.GetAll();
            ViewBag.FieldId = new SelectList(fields, "Id", "Name");

            var provinces = _provinceService.GetAll();
            ViewBag.ProvinceId = new SelectList(provinces, "Id", "Name");

            var districts = _districtService.GetAll();
            ViewBag.DistrictId = new SelectList(districts, "Id", "Name");

            var wards = _wardService.GetAll();
            ViewBag.WardId = new SelectList(wards, "Id", "Name");

            return View();
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
        public IActionResult GetAllPaging(string keyword, int fieldId, int provinceId, int districtId, int wardId, int page, int pageSize)
        {
            var model = _enterpriseService.GetAllPaging("", "", keyword, fieldId, provinceId, districtId, wardId, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _enterpriseService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllFieldsByEnterpriseId(int enterpriseId)
        {
            var model = _enterpriseFieldService.GetAllByEnterpriseId(enterpriseId)
                .Select(x=>x.FieldId);
            
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(EnterpriseViewModel enterpriseVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (enterpriseVm.Id == 0)
                    _enterpriseService.Add(enterpriseVm);
                else
                    _enterpriseService.Update(enterpriseVm);

                _enterpriseService.Save();
                return new OkObjectResult(enterpriseVm);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);
            else
            {
                _enterpriseService.Delete(id);
                _enterpriseService.Save();

                return new OkObjectResult(id);
            }
        }

        #endregion
    }
}