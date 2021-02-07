using AutoMapper;
using AutoMapper.QueryableExtensions;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.Location;
using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Data.Entities;
using BeCoreApp.Data.Enums;
using BeCoreApp.Data.IRepositories;
using BeCoreApp.Infrastructure.Interfaces;
using BeCoreApp.Utilities.Dtos;
using BeCoreApp.Utilities.Extensions;
using BeCoreApp.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BeCoreApp.Application.Implementation
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;
        private IUnitOfWork _unitOfWork;

        public TransactionService(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public PagedResult<TransactionViewModel> GetAllPaging(string keyword, string appUserId, int pageIndex, int pageSize)
        {
            var query = _transactionRepository.FindAll(x => x.AppUser);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.AppUser.FullName.Contains(keyword) || x.TransactionHas.Contains(keyword)
                || x.AddressTo.Contains(keyword) || x.AddressFrom.Contains(keyword) || x.ContractAddress.Contains(keyword));

            if (!string.IsNullOrWhiteSpace(appUserId))
                query = query.Where(x => x.AppUserId.ToString() == appUserId);

            var totalRow = query.Count();
            var data = query.OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .Select(x => new TransactionViewModel()
                {
                    Id = x.Id,
                    SponsorNo = $"VS{x.AppUser.SponsorNo.Value}",
                    AddressFrom = x.AddressFrom,
                    AddressTo = x.AddressTo,
                    Amount = x.Amount,
                    AmountTransfer = x.AmountTransfer,
                    AppUserId = x.AppUserId,
                    AppUserName = x.AppUser.UserName,
                    ContractAddress = x.ContractAddress,
                    DateCreated = x.DateCreated,
                    DecimalPlaces = x.DecimalPlaces,
                    TransactionHas = x.TransactionHas,
                    Type = x.Type,
                    TypeName = x.Type.GetDescription()
                }).ToList();

            return new PagedResult<TransactionViewModel>()
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
        }

        public void Add(TransactionViewModel model)
        {
            var transaction = new CustomerTransaction()
            {
                AddressFrom = model.AddressFrom,
                AddressTo = model.AddressTo,
                Amount = model.Amount,
                AmountTransfer = model.AmountTransfer,
                AppUserId = model.AppUserId,
                ContractAddress = model.ContractAddress,
                DateCreated = model.DateCreated,
                DecimalPlaces = model.DecimalPlaces,
                TransactionHas = model.TransactionHas,
                Type = model.Type,
            };

            _transactionRepository.Add(transaction);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
