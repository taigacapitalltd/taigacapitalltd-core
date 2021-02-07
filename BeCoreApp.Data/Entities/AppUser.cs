using BeCoreApp.Data.Enums;
using BeCoreApp.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeCoreApp.Data.Entities
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser<Guid>, IDateTracking, ISwitchable
    {
        public Guid? ReferralId { get; set; }
        public bool IsSystem { get; set; } = false;
        public string FullName { get; set; }
        public string CMND { get; set; }
        public int? SponsorNo { get; set; }
        public DateTime? BirthDay { set; get; }
        public string Avatar { get; set; }

        public string PrivateKey { get; set; }
        public string PublishKey { get; set; }

        public decimal? WalletStart { get; set; }
        public decimal? WalletGame { get; set; }
        public decimal? WalletWin { get; set; }
        public decimal? WalletValuesShare { get; set; }
        public decimal? WalletUsdtReferral { get; set; }
        public decimal? WalletUsdt { get; set; }
        public decimal? WalletEthereum { get; set; }

        public DateTime? ActivatedDate { get; set; }
        public bool? IsActivated { get; set; }
        public string WithdrawPublishKey { get; set; }
        public string CMNDImage { get; set; }
        public string BankBillImage { get; set; }
        public string BankCardImage { get; set; }
        public bool? IsUpdatedKYC { get; set; }
        public DateTime? UpdateKYCDate { get; set; }

        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string ByCreated { get; set; }
        public string ByModified { get; set; }

        public virtual ICollection<KYC> KYCs { set; get; }
        public virtual ICollection<CustomerTransaction> CustomerTransactions { set; get; }
        public virtual ICollection<Exchange> Exchanges { set; get; }
        public virtual ICollection<Referral> Referrals { set; get; }
        public virtual ICollection<Support> Supports { set; get; }
    }
}