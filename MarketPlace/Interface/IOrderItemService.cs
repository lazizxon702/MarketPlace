using MarketPlace.DTO.OrderItemDTO;

namespace MarketPlace.Interface;

public interface IOrderItemService
{
    Task<List<OrderItemReadDTO>> GetAll();
    Task<OrderItemReadDTO?> GetById(int id);
    Task<string> Create(OrderItemCreateDTO dto);
    Task<bool> Update(int id, OrderItemUpdateDTO dto);
    Task<bool> Delete(int id);
}