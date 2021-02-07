using BeCoreApp.Data.Entities;
using BeCoreApp.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeCoreApp.Data.EF.Repositories
{
    public class KYCRepository : EFRepository<KYC, int>, IKYCRepository
    {
        public KYCRepository(AppDbContext context) : base(context)
        {
        }
    }
}
