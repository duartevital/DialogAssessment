using System.ComponentModel.DataAnnotations.Schema;

namespace WeightManager.Model.Models;

public class WeightEntry
{
    public WeightEntry()
    {

    }

    public int Id { get; set; }
    public double Weight { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("WeightEntry")]
    public User? User { get; set; }
}
