using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Security.Claims;
using Bakery.ViewModels;
using Bakery.Models;

namespace Bakery.Controllers;

public class AccountController : Controller
{
    private readonly BakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signinManager;

    public AccountController(BakeryContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager)
    {
        _db = db;
        _userManager = userManager;
        _signinManager = signinManager;
    }

    [Authorize]
    public ActionResult Profile()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        else if (model.UserName == null || model.Email == null)
        {
            ModelState.AddModelError("", "You must include a username and an email.");
            return View(model);
        }
        else
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DOB = model.DOB
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                Microsoft.AspNetCore.Identity.SignInResult signinresult = await _signinManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (signinresult.Succeeded)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Login");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }
        }
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        else
        {
            string login = model.UserNameOrEmail;
            ApplicationUser user = _db.Users.FirstOrDefault(user => user.Email == model.UserNameOrEmail);
            if (user != null)
                login = user.UserName;

            Microsoft.AspNetCore.Identity.SignInResult result = await _signinManager.PasswordSignInAsync(login, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("", "There is something wrong with your login or password. Please try again.");
                return View(model);
            }
        }
    }

    public async Task<ActionResult> LogOut()
    {
        await _signinManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}