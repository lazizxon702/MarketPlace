using MarketPlace.DTO.ProductDTO;
using MarketPlace.Interface;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Services;

public class ProductService(AppDbContext db) : IProductService
{
    public async Task<List<ProductReadDTO>> GetAll()
    {
        return await db.Products
            .Where(p => !p.IsDeleted) 
            .Select(p => new ProductReadDTO
            {
                Id = p.Id,
                NameS = p.NameS,
                Description = p.Description,
                Price = p.Price,
                CreatedDate = p.CreatedDate
            })
            .ToListAsync();
    }

    
    public async Task<ProductReadDTO?> GetById(int id)
    {
        var product = await db.Products
            .Where(p => p.Id == id && !p.IsDeleted)
            .Select(p => new ProductReadDTO
            {
                Id = p.Id,
                NameS = p.NameS,
                Description = p.Description,
                Price = p.Price,
                CreatedDate = p.CreatedDate
            })
            .FirstOrDefaultAsync();

        return product;
    }

    
    public async Task<string> Create(ProductCreateDTO dto)
    {
        var product = new Product
        {
            NameS = dto.NameS,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        };

        db.Products.Add(product);
        await db.SaveChangesAsync();

        return $"Product '{product.NameS}' muvaffaqiyatli yaratildi!";
    }

   
    public async Task<bool> Update(int id, ProductUpdateDTO dto)
    {
        var product = await db.Products.FindAsync(id);
        if (product == null || product.IsDeleted) return false;

        product.NameS = dto.NameS;
        product.Description = dto.Description;
        product.Price = dto.Price;

        await db.SaveChangesAsync();
        return true;
    }

 
    public async Task<bool> Delete(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product == null || product.IsDeleted) return false;

        product.IsDeleted = true;
        await db.SaveChangesAsync();
        return true;
    }
}
