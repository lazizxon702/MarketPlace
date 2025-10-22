using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MarketPlace.DTO;
using MarketPlace.DTO.Auth;
using MarketPlace.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MarketPlace.Services;

public class AuthService(AppDbContext db, IConfiguration config) : IAuthService
{
    public async Task<string> Login(AuthLoginDTO dto)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => 
            u.Username == dto.Username && u.Password == dto.Password && !u.IsDeleted);

        if (user == null)
            return "Invalid username or password.";

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var secretKey = config["Jwt:SecretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("UserId", user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}