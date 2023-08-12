using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Security.Claims;
using Bakery.Models;

namespace Bakery.Controllers;

[Authorize]
public class MenuController : Controller
{
    private readonly BakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public MenuController(BakeryContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    /* View All Treats and Flavors, Edit and Delete*/
    public ActionResult Index()
    {
        return View();
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(TreatFlavor item)
    {
        return View();
    }

    public ActionResult Details(TreatFlavor item)
    {
        return View()
    }

    public ActionResult Edit(TreatFlavor item)
    {
        return RedirectToAction("Details", new { item = item })
    }
}