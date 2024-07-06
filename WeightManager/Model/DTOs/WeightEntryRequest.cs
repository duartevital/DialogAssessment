using System.ComponentModel.DataAnnotations;
using WeightManager.Model.Models;

namespace WeightManager.Model.DTOs;

public class WeightEntryRequest
{
    public WeightEntryRequest()
    {
        CreationDate = DateTime.Now;
    }

    public int? id { get; set; }
    [Required]
    public double Weight { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    [Required]
    public int UserId { get; set; }
}
