using MarketPlace.DTO;

namespace MarketPlace.Interface;

public interface IOrderService
{
    Task<List<OrderReadDTO>> GetAllAsync();
    Task<OrderReadDTO?> GetByIdAsync(int id);
    Task<string> CreateAsync(OrderCreateDTO dto);
    Task<bool> UpdateAsync(int id, OrderUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}