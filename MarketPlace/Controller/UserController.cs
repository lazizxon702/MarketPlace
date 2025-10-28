using MarketPlace.DTO;
using MarketPlace.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<UserReadDTO>>> GetAll()
    {
        var users = await userService.GetAll();
        return Ok(users);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserReadDTO>> GetById(int id)
    {
        var user = await userService.GetById(id);
        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<string>> CreateUser([FromBody] UserCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var message = await userService.CreateUser(dto);
        return Ok(message);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(int id, UserUpdateDTO dto)
    {
        var updated = await userService.UpdateUser(id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

 
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await userService.Delete(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
    
    
}