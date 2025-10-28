using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MarketPlace.DTO.Auth;
using MarketPlace.DTO.AuthDTO;
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
            return "Username or password is incorrect.";

        return GenerateJwtToken(user);
    }

    
    public async Task<string> SignUp(AuthRegisterDTO dto)
    {
      
        if (!dto.Email.EndsWith("@gmail.com"))
            return "Faqat '@gmail.com' email manzili qabul qilinadi!";

       
        var exists = await db.Users.AnyAsync(u => u.Username == dto.Username);
        if (exists)
            return "Bu foydalanuvchi allaqachon mavjud!";

       
        if (dto.Password != dto.PasswordVerification)
            return "Parollar mos emas!";

       
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            Password = dto.Password, 
            Role = "User",
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

     
        return $"{user.Username} muvaffaqiyatli ro‘yxatdan o‘tdi!";
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