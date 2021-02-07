using BeCoreApp.Data.Enums;
using BeCoreApp.Data.Interfaces;
using BeCoreApp.Infrastructure.SharedKernel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeCoreApp.Data.Entities
{
    [Table("KYCs")]
    public class KYC : DomainEntity<int>, IDateTracking, ISwitchable
    {
        public Guid? AppUserId { get; set; }
        public string CMNDImage { get; set; }
        public string BankBillImage { get; set; }
        public string BankCardImage { get; set; }
        public string WithdrawPublishKey { get; set; }
        public string Reason { get; set; }
        public KYCType Type { get; set; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string ByCreated { get; set; }
        public string ByModified { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { set; get; }
    }
}