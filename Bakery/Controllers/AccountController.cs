using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
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
    public async Task<ActionResult> Register()
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
    public async Task<ActionResult> Login()
    {
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> LogOut()
    {
        return RedirectToAction("Index", "Home");
    }
}