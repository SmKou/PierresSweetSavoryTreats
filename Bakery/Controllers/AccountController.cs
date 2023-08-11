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
        return RedirectToAction("Index");
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
            using (var cxt = new BakeryContext())
            {
                var user = cxt.Users.FirstOrDefault(user => user.Email == login);
                if (user != null)
                    login = user.UserName;
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signinManager.PasswordSignInAsync(model.login, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Suceeded)
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("", "There is something wrong with your email or password. Please try again.");
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