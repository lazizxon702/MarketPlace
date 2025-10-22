using MarketPlace.DTO.ProductDTO;

namespace MarketPlace.Interface;

public interface IProductService
{
    Task<List<ProductReadDTO>> GetAll();
    Task<ProductReadDTO?> GetById(int id);
    Task<string> Create(ProductCreateDTO dto);
    Task<bool> Update(int id, ProductUpdateDTO dto);
    Task<bool> Delete(int id);
}