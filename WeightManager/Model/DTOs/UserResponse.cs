namespace WeightManager.Model.DTOs;

public class UserResponse
{

    public UserResponse()
    {
        Name = string.Empty;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public double? TargetWeight { get; set; }

    public double? WeightLeft { get; set; } // Progress
}
