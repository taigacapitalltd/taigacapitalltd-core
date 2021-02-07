using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Application.ViewModels.Valuesshare;
using BeCoreApp.Data.Entities;
using BeCoreApp.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeCoreApp.Application.Interfaces
{
    public interface IKYCService
    {
        PagedResult<KYCViewModel> GetAllPaging(string keyword, int page, int pageSize);

        List<KYCViewModel> GetAllByUserId(string userId);

        Task ApprovalAsync(KYCViewModel kycVm);

        Task RevokeAsync(string appUserId);

        void Reject(KYCViewModel kycVm);

        void Lock(KYCViewModel kycVm);

        void UnLock(KYCViewModel kycVm);

        Task AddAsync(KYCViewModel kycVm);
        bool CheckShowFormRequestKYC(string userId);
        KYCViewModel GetById(int id);
        void Save();
        void Delete(int id);
    }
}
