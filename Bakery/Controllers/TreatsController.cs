using Bakery.Models;

namespace Bakery.Controllers;

public class TreatsController : Controller
{
    private readonly BakeryContext _db;

    public TreatsController(BakeryContext db)
    {
        _db = db;
    }

    public ActionResult Details(int id)
    {
        Treat model = _db.Treats
            .Include(treat => treat.Flavors)
            .ThenInclude(join => join.Flavor)
            .SingleOrDefault(treat => treat.TreatId == id);
        return View(model);
    }
}