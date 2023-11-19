
namespace _01.Entities;

public class ProductEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public CategoryEntity Category { get; set; } = null!;
    public int CategoryId { get; set; }
}
