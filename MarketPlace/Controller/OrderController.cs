using MarketPlace.DTO;
using MarketPlace.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<OrderReadDTO>>> GetAll()
    {
        var orders = await orderService.GetAllAsync();
        return Ok(orders);
    }

   
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderReadDTO>> GetById(int id)
    {
        var order = await orderService.GetByIdAsync(id);
        if (order == null)
            return NotFound("Buyurtma topilmadi");

        return Ok(order);
    }

    
    [HttpPost]
    public async Task<ActionResult<string>> CreateOrder([FromBody] OrderCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await orderService.CreateAsync(dto);

        if (result == "User not found")
            return BadRequest("Foydalanuvchi topilmadi");

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateOrder(int id, [FromBody] OrderUpdateDTO dto)
    {
        var updated = await orderService.UpdateAsync(id, dto);
        if (!updated)
            return NotFound("Buyurtma topilmadi");

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await orderService.DeleteAsync(id);
        if (!deleted)
            return NotFound("Buyurtma topilmadi");

        return NoContent();
    }
}