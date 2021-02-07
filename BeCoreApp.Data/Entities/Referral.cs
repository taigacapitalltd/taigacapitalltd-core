using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BeCoreApp.Data.Enums;
using BeCoreApp.Data.Interfaces;
using BeCoreApp.Infrastructure.SharedKernel;

namespace BeCoreApp.Data.Entities
{
    [Table("Referrals")]
    public class Referral : DomainEntity<int>
    {
        [MaxLength(500)]
        public string Description { set; get; }

        [Required]
        public string ReferralLink { set; get; }

        public int BlogId { get; set; }

        public Guid AppUserId { get; set; }

        [ForeignKey("BlogId")]
        public virtual Blog Blog { set; get; }

        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { set; get; }
    }
}
