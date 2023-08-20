using System.ComponentModel.DataAnnotations;

namespace Bakery.Models;

public class Flavor
{
    public int FlavorId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }

    public List<TreatFlavor> Treats { get; }
}