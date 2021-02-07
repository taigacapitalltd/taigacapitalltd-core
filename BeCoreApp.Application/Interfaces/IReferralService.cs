using BeCoreApp.Application.ViewModels.RealEstate;
using BeCoreApp.Application.ViewModels.Valuesshare;
using BeCoreApp.Utilities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeCoreApp.Application.Interfaces
{
    public interface IReferralService
    {
        List<ReferralGroupViewModel> GetAll(string appUserId);

        Task<ReferralViewModel> GetReferralLinkUpline(string appUserId, int blogId);

        ReferralViewModel GetByBlogId(string appUserId, int blogId);

        ReferralViewModel GetById(int id);

        void AddOrUpdate(ReferralViewModel referralVm);

        void Delete(int id);

        void Save();
    }
}
