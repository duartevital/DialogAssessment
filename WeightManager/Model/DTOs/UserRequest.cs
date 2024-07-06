namespace WeightManager.Model.DTOs;

public class UserRequest
{
    public UserRequest()
    {
        Name = string.Empty;
    }

    public int? Id { get; set; }
    public string Name { get; set; }
    public double? TargetWeight { get; set; }
}
