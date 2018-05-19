using System;
using System.ComponentModel.DataAnnotations;

namespace CuoreRosso.Models
{
    public class AccountViewModel
    {
		[Required]
		[Display(Name = "Nome")]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Sobrenome")]
        public string LastName { get; set; }

		[Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }			
        
    }
}
