using BeCoreApp.Application.ViewModels.Blog;
using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeCoreApp.Application.ViewModels.Valuesshare
{
    public class ReferralViewModel
    {
        public int Id { set; get; }
        public string ReferralLink { set; get; }

        [MaxLength(500)]
        public string Description { set; get; }

        public string BlogName { get; set; }
        public string BlogImage { get; set; }
        public string BlogDescription { get; set; }

        public int BlogId { get; set; }
        public string ReferralLinkRule { get; set; }

        public Guid AppUserId { get; set; }

        public BlogViewModel Blog { set; get; }

        public  AppUserViewModel AppUser { set; get; }

    }
}