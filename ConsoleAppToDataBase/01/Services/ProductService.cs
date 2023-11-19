
using _01.Context;
using _01.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace _01.Services;

internal class ProductService
{
    private readonly ApplicationDbContext _context;
    public ProductService()
    {
        _context = new ApplicationDbContext();
    }

    public async Task<bool> AddProductAsync(ProductEntity product)
    {
        try
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == product.Name);

            if (existingProduct != null)
            {
                return false;
            }

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == product.Category.Name);

            if (existingCategory == null)
            {
                product.Category = new CategoryEntity { Name = product.Category.Name };
                _context.Categories.Add(product.Category);
            }
            else
            {
                product.Category = existingCategory;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }


    public async Task<IEnumerable<ProductEntity>> GetAllAsync()
    {
        return await _context.Products.Include(p => p.Category).ToListAsync();
    }
}
