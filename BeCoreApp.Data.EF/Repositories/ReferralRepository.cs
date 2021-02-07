using BeCoreApp.Data.Entities;
using BeCoreApp.Data.IRepositories;

namespace BeCoreApp.Data.EF.Repositories
{
    public class ReferralRepository : EFRepository<Referral, int>, IReferralRepository
    {
        public ReferralRepository(AppDbContext context) : base(context)
        {
        }
    }
}
