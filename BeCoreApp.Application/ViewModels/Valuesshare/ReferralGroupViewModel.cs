using BeCoreApp.Application.ViewModels.Blog;
using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeCoreApp.Application.ViewModels.Valuesshare
{
    public class ReferralGroupViewModel
    {
        public ReferralGroupViewModel()
        {
            Referrals = new List<ReferralViewModel>();
        }

        public string Name { set; get; }
        public string Description { get; set; }

        public List<ReferralViewModel> Referrals { set; get; }
    }
}