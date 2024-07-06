using System.ComponentModel.DataAnnotations.Schema;

namespace WeightManager.Model.Models;
public class User
{
    public User()
    {
        Name = string.Empty;
        WeightEntry = new List<WeightEntry>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public double? TargetWeight { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<WeightEntry> WeightEntry { get; set; }
}
