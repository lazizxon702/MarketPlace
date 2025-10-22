using MarketPlace.DTO.CategoryDTO;
using MarketPlace.Interface;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Services;

public class CategoryService(AppDbContext db) : ICategoryService
{
    private readonly AppDbContext _db = db;

    
    public async Task<List<CategoryReadDTO>> GetAll()
    {
        return await _db.Categories
            .Where(c => !c.IsDeleted) 
            .Select(c => new CategoryReadDTO
            {
                Id = c.Id,
                Keyword = c.Keyword
            })
            .ToListAsync();
    }

 
    public async Task<List<CategoryReadDTO>> GetById(int id)
    {
        var category = await _db.Categories
            .Where(c => c.Id == id && !c.IsDeleted)
            .Select(c => new CategoryReadDTO
            {
                Id = c.Id,
                Keyword = c.Keyword
            })
            .FirstOrDefaultAsync();
        if (category == null) return null;
        
        return [category];
    }

   
    public async Task<string> Create(CategoryCreateDTO dto)
    {
        var category = new Category
        {
            Keyword = dto.Keyword,
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        return $"Category '{category.Keyword}' muvaffaqiyatli yaratildi!";
    }

   
    public async Task<bool> Update(int id, CategoryUpdateDTO dto)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category.IsDeleted) return false;

        category.Keyword = dto.Keyword;

        await _db.SaveChangesAsync();
        return true;
    }

   
    public async Task<bool> Delete(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category == null || category.IsDeleted) return false;

        category.IsDeleted = true;
        await _db.SaveChangesAsync();
        return true;
    }
}
