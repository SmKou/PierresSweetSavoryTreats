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
    public async Task<ActionResult> Profile()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser user = await _userManager.FindByIdAsync(userId);

        AccountViewModel model = new AccountViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            DOB = user.DOB,
            UserName = user.UserName,
            Email = user.Email
        };

        if (TempData["Confirmation"] != null)
            ViewBag.Confirmation = TempData["Confirmation"];

        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Profile(AccountViewModel avm)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser user = await _userManager.FindByIdAsync(userId);

        if (avm.UserName != user.UserName && avm.Email != user.Email)
        {
            ModelState.AddModelError("", "Cannot change both username and email.");
            return View(avm);
        }

        if (avm.FirstName != user.FirstName)
            user.FirstName = avm.FirstName;

        if (avm.LastName != user.LastName)
            user.LastName = avm.LastName;

        if (avm.DOB != user.DOB)
            user.DOB = avm.DOB;

        if (avm.Email != user.Email)
            user.Email = avm.Email;
        else if (avm.UserName != user.UserName)
            user.UserName = avm.UserName;

        IdentityResult result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(avm);
        }

        if (!string.IsNullOrEmpty(avm.Password) && !string.IsNullOrEmpty(avm.PasswordConfirm))
        {
            IdentityResult pwresult = await _userManager.ChangePasswordAsync(user, avm.Password, avm.PasswordConfirm);
            if (!pwresult.Succeeded)
            {
                foreach (IdentityError error in pwresult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(avm);
            }
        }

        return RedirectToAction("Profile");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register(AccountViewModel rvm)
    {
        if (!ModelState.IsValid)
            return View(rvm);
        else if (rvm.UserName == null || rvm.Email == null)
        {
            ModelState.AddModelError("", "You must include a username and an email.");
            return View(rvm);
        }
        else
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = rvm.UserName,
                Email = rvm.Email,
                FirstName = rvm.FirstName,
                LastName = rvm.LastName,
                DOB = rvm.DOB
            };
            IdentityResult result = await _userManager.CreateAsync(user, rvm.Password);
            if (result.Succeeded)
            {
                Microsoft.AspNetCore.Identity.SignInResult signinresult = await _signinManager.PasswordSignInAsync(rvm.UserName, rvm.Password, isPersistent: true, lockoutOnFailure: false);
                if (signinresult.Succeeded)
                    return RedirectToAction("Index", "Inventory");
                else
                    return RedirectToAction("Login");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(rvm);
            }
        }
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel lvm)
    {
        if (!ModelState.IsValid)
            return View(lvm);
        else
        {
            string login = lvm.UserNameOrEmail;
            ApplicationUser user = _db.Users.FirstOrDefault(user => user.Email == lvm.UserNameOrEmail);
            if (user != null)
                login = user.UserName;

            Microsoft.AspNetCore.Identity.SignInResult result = await _signinManager.PasswordSignInAsync(login, lvm.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Inventory");
            else
            {
                ModelState.AddModelError("", "There is something wrong with your login or password. Please try again.");
                return View(lvm);
            }
        }
    }

    public async Task<ActionResult> LogOut()
    {
        await _signinManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}