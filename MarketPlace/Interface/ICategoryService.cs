using MarketPlace.DTO.CategoryDTO;

namespace MarketPlace.Interface;

public interface ICategoryService
{
    Task<List<CategoryReadDTO>>  GetAll();
    Task<List<CategoryReadDTO>>  GetById(int id);
    Task<string> Create(CategoryCreateDTO dto);
    Task<bool> Update(int id, CategoryUpdateDTO dto);
    Task<bool> Delete(int id);
}