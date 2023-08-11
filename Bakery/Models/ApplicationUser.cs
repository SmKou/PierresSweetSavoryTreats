using Microsoft.AspNetCore.Identity;

namespace Bakery.Models;

public class ApplicationUser : IdentityUser
{
    /*  Inherited
        Id
        UserName
        -> NormalizedUserName
        Email
        -> NormalizedEmail
    */
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DOB { get; set; }
}