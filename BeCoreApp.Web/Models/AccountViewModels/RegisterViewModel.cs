using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeCoreApp.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { set; get; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string CMND { get; set; }
        [Required]
        public string SponsorNo { get; set; }
        //[Required]
        //public int? CountryId { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}