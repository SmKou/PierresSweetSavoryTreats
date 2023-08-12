using Microsoft.AspNetCore.Authorization;
using Bakery.Models;

namespace Bakery.Controllers;

[Authorize]
public class InventoryController : Controller
{
    public readonly BakeryContext _db;

    public InventoryController(BakeryContext db)
    {
        _db = db;
    }

    public ActionResult Index()
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Flavor> flavors = _db.Flavors.ToList();
        List<Treat> treats = _db.Treats.ToList();
        model.Add("Flavors", flavors);
        model.Add("Treats", treats);
        return View();
    }
}