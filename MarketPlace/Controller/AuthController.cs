using MarketPlace.DTO;
using MarketPlace.DTO.Auth;
using MarketPlace.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthLoginDTO dto)
    {
        var token = await authService.Login(dto);

        if (token == "Invalid username or password.")
            return Unauthorized(new { message = token });

        return Ok(new { token });
    }
}