using MarketPlace.DTO;
using MarketPlace.Interface;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Services;

public class UserService(AppDbContext db) : IUserService
{
    public async Task<List<UserReadDTO>> GetAll()
    {                                                                                                              
        var users = await db.Users                                                                                 
            .Where(u => !u.IsDeleted)                                                                              
            .Select(u => new UserReadDTO                                                                           
            {                                                                                                      
                Id = u.Id,                                                                                         
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,                                                                                     
                CreatedDate = u.CreatedDate                                                                        
            })                                                                                                     
            .ToListAsync();

        return users;
    }

    public async Task<UserReadDTO> GetById(int id)
    {
        var user = await db.Users
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (user == null)
            return null;

        return new UserReadDTO
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            CreatedDate = user.CreatedDate
        };
    }

    public async Task<string> CreateUser(UserCreateDTO dto)
    {
        var exists = await db.Users.AnyAsync(u => 
            u.Username == dto.Username || u.Email == dto.Email);

        if (exists)
            return "User with this username or email already exists.";

        var newUser = new User
        {
            Username = dto.Username,
            Password = dto.Password,
            Email = dto.Email,
            Role = dto.Role,
            CreatedDate = DateTime.UtcNow
        };

        db.Users.Add(newUser);
        await db.SaveChangesAsync();

        return "User successfully created.";
    }


    public async Task<bool> UpdateUser(int id, UserUpdateDTO dto)
    {
        var user = await db.Users.FindAsync(id);
        if (user == null || user.IsDeleted)
            return false;

        user.Username = dto.FullName;
        user.Password = dto.Password;
        user.Email = dto.Email;
        user.Role = dto.Role;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var user = await db.Users
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (user == null)
            return false;

       
        user.IsDeleted = true;
        await db.SaveChangesAsync();
        return true;
    }

  
}
