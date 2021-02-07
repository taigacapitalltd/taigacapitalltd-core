using BeCoreApp.Application.ViewModels.Blog;
using BeCoreApp.Application.ViewModels.Common;
using BeCoreApp.Application.ViewModels.Valuesshare;
using BeCoreApp.Data.Enums;
using System;
using System.Collections.Generic;

namespace BeCoreApp.Application.ViewModels.System
{
    public class AppUserViewModel
    {
        public AppUserViewModel()
        {
            Roles = new List<string>();
            KYCs = new List<KYCViewModel>();
            Transactions = new List<TransactionViewModel>();
            Exchanges = new List<ExchangeViewModel>();
            Referrals = new List<ReferralViewModel>();
            Supports = new List<SupportViewModel>();
        }

        public Guid? Id { set; get; }
        public string ID { get; set; }
        public string Referal { get; set; }
        public Guid? ReferralId { get; set; }
        public bool IsSystem { get; set; }
        public string FullName { set; get; }
        public string BirthDay { set; get; }
        public string Email { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public string PhoneNumber { set; get; }
        public string CMND { get; set; }
        public string Avatar { get; set; }
        public bool EmailConfirmed { get; set; }
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

        public string ReferalLink { get; set; }


        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string ByCreated { get; set; }
        public string ByModified { get; set; }

        public List<string> Roles { get; set; }
        public List<KYCViewModel> KYCs { set; get; }
        public List<TransactionViewModel> Transactions { set; get; }
        public List<ExchangeViewModel> Exchanges { set; get; }
        public List<ReferralViewModel> Referrals { set; get; }
        public List<SupportViewModel> Supports { set; get; }
    }
}
