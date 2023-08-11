using System.ComponentModel.DataAnnotations;

namespace Bakery.Models;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public string DOB { get; set; }

    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$", ErrorMessage = "Your password must contain at least 8 characters, a capital letter, a lowercase letter, a number and a special character.")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The passwords entered do not match.")]
    public string PasswordConfirm { get; set; }
}