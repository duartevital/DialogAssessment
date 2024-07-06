using WeightManager.Model.Models;

namespace WeightManager.Model.DTOs;

public class WeightEntryResponse
{
    public WeightEntryResponse()
    {

    }

    public int Id { get; set; }
    public double Weight { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    public int UserId { get; set; }
}
