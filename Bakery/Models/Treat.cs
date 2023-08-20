using System.ComponentModel.DataAnnotations;

namespace Bakery.Models;

public class Treat
{
    public int TreatId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }

    public List<TreatFlavor> Flavors { get; }
}