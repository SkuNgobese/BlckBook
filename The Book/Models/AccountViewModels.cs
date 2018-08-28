using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace The_Book.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Please choose role, Teacher or Student.")]
        [DisplayName("Role")]
        public string role { get; set; }

        [Required(ErrorMessage = "Your first name cannot be empty.")]
        [MaxLength(50),MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Spaces, special characters & numbers not allowed on {0}")]
        [DisplayName("First Name")]
        public string firstName { get; set; }

        [MaxLength(50), MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Spaces, special characters & numbers not allowed on {0}")]
        [DisplayName("Middle Name")]
        public string middleName { get; set; }

        [Required(ErrorMessage = "Your Last Name cannot be empty.")]
        [MaxLength(50), MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Spaces, special characters & numbers not allowed on {0}")]
        [DisplayName("Last Name")]
        public string lastName { get; set; }
        
        [Required]
        [EmailAddress]
        [System.Web.Mvc.Remote("IsAlreadySigned", "Account", ErrorMessage ="Looks like you already are part of BB family, proceed to login or try forgot password to reset ur password.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "User Photo")]
        public byte[] userPhoto { get; set; }

        [NotMapped]
        [ValidatePicture(ErrorMessage ="Please choose a Picture not more than 1MB.")]
        public HttpPostedFileBase poImgFile { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your School.")]
        public string schoolname { get; set; }
    }
    public class ChangeDpViewModel
    {
        [NotMapped]
        [ValidatePicture(ErrorMessage = "Please choose a Picture not more than 1MB.")]
        public HttpPostedFileBase poImgFile { get; set; }
    }
    public class ManagerRegisterViewModel
    {
        [Required(ErrorMessage = "For what School is the manager?")]
        public string schoolname { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
