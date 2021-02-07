using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Models.ProjectViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace BeCoreApp.Controllers
{
    public class ProjectController : Controller
    {
        IProjectCategoryService _projectCategoryService;
        IProjectService _projectService;
        IConfiguration _configuration;
        public ProjectController(
            IProjectCategoryService projectCategoryService, IProjectService projectService, IConfiguration configuration)
        {
            _projectCategoryService = projectCategoryService;
            _projectService = projectService;
            _configuration = configuration;
        }

        [Route("du-an.html")]
        public IActionResult Index()
        {
            var catalog = new CatalogViewModel();
            catalog.ProjectCategoryVMs = _projectCategoryService.GetAll();

            foreach (var projectCategory in catalog.ProjectCategoryVMs)
            {
                var Projects = _projectService.GetAllByProjectCategoryId(projectCategory.Id).Where(x => x.HomeFlag.Value);
                catalog.ProjectVMs.AddRange(Projects);
            }

            return View(catalog);
        }

        [Route("{alias}-pjc.{id}.html")]
        public IActionResult ProjectCategory(int id, int? pageSize, int page = 1)
        {
            var catalog = new CatalogViewModel();
            catalog.ProjectCategoryVM = _projectCategoryService.GetById(id);
            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("PageSize");

            catalog.PageSize = pageSize;
            catalog.Data = _projectService.GetAllPaging("", "", "", 0, 0, 0, 0, id, page, pageSize.Value);

            return View(catalog);
        }

        [Route("{alias}-pj.{id}.html", Name = "ProjectDetail")]
        public IActionResult ProjectDetail(int id)
        {
            var model = new DetailViewModel();
            model.ProjectVM = _projectService.GetById(id);
            model.ProjectImageVMs = _projectService.GetImages(id);
            model.ProjectLibraryVMs = _projectService.GetLibraries(id);
            model.ProjectCategoryVM = _projectCategoryService.GetById(model.ProjectVM.ProjectCategoryId);

            return View(model);
        }
    }
}