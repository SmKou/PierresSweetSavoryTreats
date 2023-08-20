using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bakery.Models;

namespace Bakery.Controllers;

[Authorize]
public class TreatsController : Controller
{
    private readonly BakeryContext _db;

    public TreatsController(BakeryContext db)
    {
        _db = db;
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Treat model)
    {
        if (!ModelState.IsValid)
            return View(model);
        bool hasEntity = _db.Treats.Any(entity => entity.Name == model.Name);
        if (hasEntity)
        {
            ModelState.AddModelError("", "Treat already exists.");
            return View(model);
        }
        _db.Treats.Add(model);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = model.TreatId });
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
        TreatFlavor model = new TreatFlavor();
        model.TreatId = id;
        model.Treat = _db.Treats
            .Include(treat => treat.Flavors)
            .ThenInclude(join => join.Flavor)
            .SingleOrDefault(treat => treat.TreatId == id);
        if (_db.Flavors.Count() != 0)
            ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
        return View(model);
    }

    [HttpPost]
    public ActionResult AddFlavor(TreatFlavor model)
    {
        if (!ModelState.IsValid)
            return View(model);
        bool hasRelation = _db.TreatFlavors.Any(join => join.FlavorId == model.FlavorId && join.TreatId == model.TreatId);
        if (!hasRelation)
        {
            _db.TreatFlavors.Add(model);
            _db.SaveChanges();
        }
        return RedirectToAction("Details", new { id = model.TreatId });
    }

    [HttpPost]
    public ActionResult RemoveFlavor(int joinId)
    {
        TreatFlavor rm = _db.TreatFlavors.SingleOrDefault(join => join.TreatFlavorId == joinId);
        int treatId = rm.TreatId;
        _db.TreatFlavors.Remove(rm);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = treatId });
    }

    public ActionResult Edit(int id)
    {
        Treat model = _db.Treats
            .Include(treat => treat.Flavors)
            .ThenInclude(flavors => flavors.Flavor)
            .SingleOrDefault(treat => treat.TreatId == id);
        return View(model);
    }

    [HttpPost]
    public ActionResult Edit(Treat model)
    {
        if (!ModelState.IsValid)
            return View(model);
        _db.Treats.Update(model);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = model.TreatId });
    }

    public ActionResult Delete(int id)
    {
        Treat rm = _db.Treats.SingleOrDefault(treat => treat.TreatId == id);
        _db.Treats.Remove(rm);
        _db.SaveChanges();
        return RedirectToAction("Index", "Inventory");
    }
}