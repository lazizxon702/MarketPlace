using MarketPlace.DTO;
using MarketPlace.Interface;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Services;

public class OrderService(AppDbContext db) : IOrderService
{
    public async Task<List<OrderReadDTO>> GetAllAsync()
    {
        return await db.Orders
            .Include(o => o.User)
            .Select(o => new OrderReadDTO
            {
                Id = o.Id,
                UserFullName = o.User.Username,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                PaymentType = o.PaymentType,
                Address = o.Address
            })
            .ToListAsync();
    }

    public async Task<OrderReadDTO?> GetByIdAsync(int id)
    {
        var order = await db.Orders
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

        if (order == null) return null;

        return new OrderReadDTO
        {
            Id = order.Id,
            UserFullName = order.User.Username,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            PaymentType = order.PaymentType,
            Address = order.Address
        };
    }

    public async Task<string> CreateAsync(OrderCreateDTO dto)
    {
        var user = await db.Users.FindAsync(dto.UserId);
        if (user == null) return "User not found";

        var order = new Order
        {
            UserId = dto.UserId,
            TotalAmount = dto.TotalAmount,
            PaymentType = dto.PaymentType,
            Address = dto.Address,
            Status = "Pending",
            OrderDate = DateTime.UtcNow
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();

        return "Order created successfully";
    }

    public async Task<bool> UpdateAsync(int id, OrderUpdateDTO dto)
    {
        var order = await db.Orders.FindAsync(id);
        if (order == null || order.IsDeleted) return false;

        order.TotalAmount = dto.TotalAmount;
        order.Status = dto.Status;
        order.PaymentType = dto.PaymentType;
        order.Address = dto.Address;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await db.Orders.FindAsync(id);
        if (order == null || order.IsDeleted) return false;

        order.IsDeleted = true;
        await db.SaveChangesAsync();
        return true;
    }
}
