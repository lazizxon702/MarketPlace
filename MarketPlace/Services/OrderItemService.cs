using MarketPlace.DTO.OrderItemDTO;
using MarketPlace.Interface;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Services;

public class OrderItemService(AppDbContext db) : IOrderItemService
{
    
    public async Task<List<OrderItemReadDTO>> GetAll()
    {
        var items = await db.OrderItems
            .Include(oi => oi.Product) 
            .ToListAsync();

       
        return items.Select(i => new OrderItemReadDTO
        {
            Id = i.Id,
            ProductName = i.Product.NameS,
            Quantity = i.Quantity,
            Price = i.Price
        }).ToList();
    }

   
    public async Task<OrderItemReadDTO?> GetById(int id)
    {
        var item = await db.OrderItems
            .Include(oi => oi.Product)
            .FirstOrDefaultAsync(oi => oi.Id == id);

        if (item == null) return null;

        return new OrderItemReadDTO
        {
            Id = item.Id,
            ProductName = item.Product.NameS,
            Quantity = item.Quantity,
            Price = item.Price
        };
    }

   
    public async Task<string> Create(OrderItemCreateDTO dto)
    {
        
        var order = await db.Orders.FindAsync(dto.OrderId);
        if (order == null) return "Buyurtma topilmadi";

        var product = await db.Products.FindAsync(dto.ProductId);
        if (product == null) return "Mahsulot topilmadi";

        
        var orderItem = new OrderItem
        {
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            Price = product.Price * dto.Quantity
        };

        db.OrderItems.Add(orderItem);

        
        order.TotalAmount += orderItem.Price;

        await db.SaveChangesAsync();
        return "Buyurtma itemi qo‘shildi";
    }

   
    public async Task<bool> Update(int id, OrderItemUpdateDTO dto)
    {
        var item = await db.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.Product)
            .FirstOrDefaultAsync(oi => oi.Id == id);

        if (item == null) return false;

        
        item.Order.TotalAmount -= item.Price;  
        item.Quantity = dto.Quantity;
        item.Price = item.Product.Price * dto.Quantity;
        item.Order.TotalAmount += item.Price;  

        await db.SaveChangesAsync();
        return true;
    }

   
    public async Task<bool> Delete(int id)
    {
        var item = await db.OrderItems
            .Include(oi => oi.Order)
            .FirstOrDefaultAsync(oi => oi.Id == id);

        if (item == null) return false;

        
        item.Order.TotalAmount -= item.Price;

        db.OrderItems.Remove(item); 
        await db.SaveChangesAsync();

        return true;
    }
}
