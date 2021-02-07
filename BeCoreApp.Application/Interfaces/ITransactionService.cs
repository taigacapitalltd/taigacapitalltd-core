using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Data.Enums;
using BeCoreApp.Utilities.Dtos;
using System.Collections.Generic;

namespace BeCoreApp.Application.Interfaces
{
    public interface ITransactionService
    {
        PagedResult<TransactionViewModel> GetAllPaging(string keyword, string appUserId, int pageIndex, int pageSize);

        void Add(TransactionViewModel Model);

        void Save();
    }
}
