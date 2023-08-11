using Bakery.Models;

namespace Bakery.Controllers;

public class HomeController : Controller
{
    private readonly BakeryContext _db;

    public HomeController(BakeryContext db)
    {
        _db = db;
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
}