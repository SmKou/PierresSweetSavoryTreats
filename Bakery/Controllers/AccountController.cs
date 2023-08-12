using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Security.Claims;
using Bakery.ViewModels;
using Bakery.Models;

namespace Bakery.Controllers;

[Authorize]
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

    /* View All Treats and Flavors, Edit and Delete, [link] Create item */
    public ActionResult Index()
    {
        return View();
    }

    /* Details, Create treat and/or flavor, add one to other */
    public ActionResult Create(TreatFlavor item, bool type)
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(TreatFlavor item, bool type, bool bind)
    {
        return View();
    }

    /* Account details and edit */
    public ActionResult Details(int id)
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    // [AllowAnonymous]
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
                return RedirectToAction("Index");
            else
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }
        }
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    // [AllowAnonymous]
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