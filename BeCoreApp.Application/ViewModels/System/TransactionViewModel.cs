using BeCoreApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeCoreApp.Application.ViewModels.System
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public string SponsorNo { get; set; }

        public string TransactionHas { get; set; }

        public string ContractAddress { get; set; }

        public string AddressTo { get; set; }

        public string AddressFrom { get; set; }

        public decimal Amount { get; set; }

        public decimal AmountTransfer { get; set; }

        public int DecimalPlaces { get; set; }

        public TransactionType Type { get; set; }

        public string TypeName { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid AppUserId { get; set; }

        public string AppUserName { get; set; }

        public AppUserViewModel AppUser { set; get; }
    }
}
