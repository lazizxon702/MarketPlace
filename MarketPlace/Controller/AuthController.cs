using MarketPlace.DTO.Auth;
using MarketPlace.DTO.AuthDTO;
using MarketPlace.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] AuthRegisterDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.SignUp(dto);
        
        if (result.Contains("muvaffaqiyatli ro‘yxatdan o‘tdi"))
        {
            return Ok(new { message = result });
        }
        else
        {
            return BadRequest(new { error = result });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthLoginDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var tokenOrError = await authService.Login(dto);

        if (tokenOrError.Contains("incorrect"))
            return BadRequest(new { error = tokenOrError });

        return Ok(new { token = tokenOrError });
    }
    
    
}
