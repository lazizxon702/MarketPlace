using MarketPlace.DTO.OrderItemDTO;
using MarketPlace.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderItemController(IOrderItemService orderItemService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<OrderItemReadDTO>>> GetAll()
    {
        var orderItems = await orderItemService.GetAll();
        return Ok(orderItems);
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderItemReadDTO>> GetById(int id)
    {
        var orderItem = await orderItemService.GetById(id);
        if (orderItem == null)
            return NotFound("Order item topilmadi");

        return Ok(orderItem);
    }

    
    [HttpPost]
    public async Task<ActionResult<string>> CreateOrderItem([FromBody] OrderItemCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await orderItemService.Create(dto);

        if (result == "Order not found" || result == "Product not found")
            return BadRequest(result);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateOrderItem(int id, [FromBody] OrderItemUpdateDTO dto)
    {
        var updated = await orderItemService.Update(id, dto);
        if (!updated)
            return NotFound("Order item topilmadi");

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await orderItemService.Delete(id);
        if (!deleted)
            return NotFound("Order item topilmadi");

        return NoContent();
    }
}