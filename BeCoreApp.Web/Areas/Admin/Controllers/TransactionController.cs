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
    public class TransactionController : BaseController
    {
        public ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            string appUserId = string.Empty;

            var roleName = User.GetSpecificClaim("RoleName");
            if (roleName.ToLower() == "customer")
            {
                appUserId = User.GetSpecificClaim("UserId");
            }

            var model = _transactionService.GetAllPaging(keyword, appUserId, page, pageSize);
            return new OkObjectResult(model);
        }
    }
}