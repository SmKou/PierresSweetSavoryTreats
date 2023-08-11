using Bakery.Models;

namespace Bakery.Controllers;

public class FlavorsController : Controller
{
    private readonly BakeryContext _db;

    public FlavorsController(BakeryContext db)
    {
        _db = db;
    }

    public ActionResult Details(int id)
    {
        Flavor model = _db.Flavors
            .Include(flavor => flavor.Treats)
            .ThenInclude(join => join.Treat)
            .SingleOrDefault(flavor => flavor.FlavorId == id);
        return View(model);
    }
}