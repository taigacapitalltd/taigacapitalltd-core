using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.Blog;
using BeCoreApp.Application.ViewModels.Enterprise;
using BeCoreApp.Application.ViewModels.Location;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeCoreApp.Areas.Admin.Controllers
{
    public class FieldController : BaseController
    {
        public IFieldService _fieldService;

        public FieldController(IFieldService fieldService)
        {
            _fieldService = fieldService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _fieldService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _fieldService.GetAllPaging("","",keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(FieldViewModel fieldVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (fieldVm.Id == 0)
                _fieldService.Add(fieldVm);
            else
                _fieldService.Update(fieldVm);

            _fieldService.Save();
            return new OkObjectResult(fieldVm);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            _fieldService.Delete(id);
            _fieldService.Save();

            return new OkObjectResult(id);
        }
    }
}