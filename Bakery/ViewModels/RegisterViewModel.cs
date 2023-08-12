using System.ComponentModel.DataAnnotations;

namespace Bakery.ViewModels;

public class RegisterViewModel : AccountViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The passwords entered do not match.")]
    public new string PasswordConfirm { get; set; }
}