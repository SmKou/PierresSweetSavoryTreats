using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bakery.Models;

namespace Bakery.Controllers;

[Authorize]
public class FlavorsController : Controller
{
    private readonly BakeryContext _db;

    public FlavorsController(BakeryContext db)
    {
        _db = db;
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Flavor model)
    {
        bool hasEntity = _db.Flavors.Any(entity => entity.Name == model.Name);
        if (hasEntity)
        {
            ModelState.AddModelError("", "Flavor already exists.");
            return View(model);
        }
        _db.Flavors.Add(model);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = model.FlavorId })
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
        TreatFlavor model = new TreatFlavor();
        model.FlavorId = id;
        model.Flavor = _db.Flavors
            .Include(flavor => flavor.Treats)
            .ThenInclude(join => join.Treat)
            .SingleOrDefault(flavor => flavor.FlavorId == id);
        if (_db.Treats.Count() != 0)
            ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
        return View(model);
    }

    [HttpPost]
    public ActionResult AddTreat(TreatFlavor model)
    {
        bool hasRelation = _db.TreatFlavors.Any(join => join.TreatId == model.TreatId && join.FlavorId == model.FlavorId);
        if (!hasRelation)
        {
            _db.TreatFlavors.Add(model);
            _db.SaveChanges();
        }
        return RedirectToAction("Details", new { id = model.FlavorId });
    }

    [HttpPost]
    public ActionResult RemoveTreat(int joinId)
    {
        TreatFlavor rm = _db.TreatFlavors.SingleOrDefault(join => join.TreatFlavorId == joinId);
        int flavorId = rm.FlavorId;
        _db.TreatFlavors.Remove(rm);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = flavorId });
    }

    public ActionResult Edit(int id)
    {
        Flavor model = _db.Flavors
            .Include(flavor => flavor.Treats)
            .ThenInclude(treats => treats.Treat)
            .SingleOrDefault(flavor => flavor.FlavorId == id);
        return View(model);
    }

    [HttpPost]
    public ActionResult Edit(Flavor model)
    {
        _db.Flavors.Update(model);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = model.FlavorId });
    }

    public ActionResult Delete(int id)
    {
        Flavor rm = _db.Flavors.SingleOrDefault(flavor => flavor.FlavorId == id);
        _db.Flavors.Remove(flavor);
        _db.SaveChanges();
        return RedirectToAction("Index", "Inventory");
    }
}