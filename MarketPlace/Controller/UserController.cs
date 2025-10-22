using MarketPlace.DTO;
using MarketPlace.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserReadDTO>>> GetAll()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserReadDTO>> GetById(int id)
    {
        var user = await _userService.GetById(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<string>> CreateUser([FromBody] UserCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var message = await _userService.CreateUser(dto);
        return Ok(message);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(int id, UserUpdateDTO dto)
    {
        var updated = await _userService.UpdateUser(id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _userService.Delete(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}