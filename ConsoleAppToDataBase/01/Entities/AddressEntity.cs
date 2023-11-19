
using System.ComponentModel.DataAnnotations;

namespace _01.Entities;

public class AddressEntity
{
    [Key]
    public int Id { get; set; }
    public string StreetName { get; set; } = null!;
    public string StreetNumber { get; set; } = null!;
}
