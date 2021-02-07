using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Models.ProductViewModels;
using BeCoreApp.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace BeCoreApp.Controllers
{
    public class ProductController : Controller
    {
        IBlogCategoryService _blogCategoryService;
        IBlogService _blogService;
        IProductService _productService;
        IBillService _billService;
        IConfiguration _configuration;
        public ProductController(IProductService productService, IBlogCategoryService blogCategoryService,
        IBlogService blogService, IConfiguration configuration, IBillService billService)
        {
            _blogCategoryService = blogCategoryService;
            _blogService = blogService;
            _productService = productService;
            _configuration = configuration;
            _billService = billService;
        }

        [Route("{alias}-pc.{id}.html")]
        public IActionResult Catalog(int id, int? pageSize, int page = 1)
        {
            var catalog = new CatalogViewModel();
            catalog.BlogCategory = _blogCategoryService.GetById(id);
            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("ProductPageSize");

            catalog.PageSize = pageSize;
            catalog.Data = _productService.GetAllPaging("", "", "", id, page, pageSize.Value);
            catalog.Tags = _blogService.GetListTagByType(CommonConstants.ProductTag);

            return View(catalog);
        }

        [Route("chu-de-san-pham/{id}.html")]
        public IActionResult ProductTag(string id, int? pageSize, int page = 1)
        {
            var catalog = new CatalogViewModel();
            catalog.Tag = _blogService.GetTagById(id);
            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("ProductPageSize");

            catalog.PageSize = pageSize;
            catalog.Data = _productService.GetAllByTagId(id, page, pageSize.Value);
            catalog.Tags = _productService.GetListTagByType(CommonConstants.ProductTag);

            return View(catalog);
        }



        [Route("{alias}-p.{id}.html", Name = "ProductDetail")]
        public IActionResult Details(int id)
        {
            var model = new DetailViewModel();
            model.Product = _productService.GetById(id);
            model.ProductTags = _productService.GetProductTags(id);
            model.RelatedProducts = _productService.GetRelatedProducts(id, 9);
            model.Tags = _productService.GetListTagByType(CommonConstants.ProductTag);

            return View(model);
        }
    }
}