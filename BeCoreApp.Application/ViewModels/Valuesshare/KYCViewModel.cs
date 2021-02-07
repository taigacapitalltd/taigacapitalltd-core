using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeCoreApp.Application.ViewModels.Valuesshare
{
    public class KYCViewModel
    {
        public int Id { get; set; }
        public string ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CMND { get; set; }
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

        public AppUserViewModel AppUser { set; get; }
    }
}