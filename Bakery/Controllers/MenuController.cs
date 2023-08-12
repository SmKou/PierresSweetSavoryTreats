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

    public ActionResult Index()
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Treat> treats = _db.Treats.ToList();
        List<Flavor> flavors = _db.Flavors.ToList();
        model.Add("Treats", treats);
        model.Add("Flavors", flavors);
        return View(model);
    }

    public ActionResult Create()
    {
        if (TempData["Error"] != null)
            ViewBag.Error = TempData["Error"];
        
        return View();
    }

    [HttpPost]
    public ActionResult Create(TreatFlavor item, bool bind)
    {
        ()
        return View();
    }

    public ActionResult Details(int id, bool type)
    {
        TreatFlavor model = new TreatFlavor();
        if (type)
        {
            Flavor flavor = _db.Flavors
                .Include(itm => itm.Treats)
                .ThenInclude(treat => treat.Treat)
                .SingleOrDefault(itm => itm.FlavorId == id);
            model.FlavorId = flavor.FlavorId;
            model.Flavor = flavor;
            ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
        }
        else
        {
            Treat treat = _db.Treats
                .Include(itm => itm.Flavors)
                .ThenInclude(flavor => flavor.Flavor)
                .SingleOrDefault(itm => itm.TreatId == id);
            model.TreatId = treat.TreatId;
            model.Treat = treat;
            ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
        }
        return View(model);
    }

    public ActionResult Edit(TreatFlavor item)
    {
        if (item.Flavor != null && item.Treat != null)
            return RedirectToAction("Create");

        if (item.Treat == null)
        {
            Flavor orig = _db.Flavors.SingleOrDefault(itm => itm.FlavorId == item.FlavorId);
            if (orig.Name != item.Flavor.Name || orig.Description != item.Flavor.Description)
                _db.Flavors.Update(item.Flavor);
        }
        else
        {
            Treat orig = _db.Treats.SingleOrDefault(itm => itm.TreatId == item.TreatId);
            if (orig.Name != item.Treat.Name || orig.Description != item.Treat.Description)
                _db.Treats.Update(item.Treat);
        }

        if (item.FlavorId != null && item.TreatId != null)
        {
            bool hasEntity = _db.TreatFlavors.Any(join => join.FlavorId == item.FlavorId && join.TreatId == item.TreatId);
            if (!hasEntity)
                _db.TreatFlavors.Add(new TreatFlavor()
                {
                    FlavorId = item.FlavorId,
                    TreatId = item.TreatId
                });
        }

        _db.SaveChanges();
        return RedirectToAction("Details", new { item = item });
    }

    public ActionResult Delete(int id, bool type)
    {
        if (type)
            _db.Flavors.Remove(_db.Flavors
                .SingleOrDefault(itm => itm.FlavorId == id)
            );
        else
            _db.Treats.Remove(_db.Treats
                .SingleOrDefault(itm => itm.TreatId == id)
            );
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public ActionResult DeleteJoin(int id, bool type)
    {
        TreatFlavor join = _db.TreatFlavors.SingleOrDefault(tf => tf.TreatFlavorId == id);
        int itemId = type ? join.FlavorId : join.TreatId;
        _db.TreatFlavors.Remove(join);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = itemId, type = type });
    }
}