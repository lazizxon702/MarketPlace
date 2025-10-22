using MarketPlace.DTO;

namespace MarketPlace.Interface
{
    public interface IUserService
    {
        Task<List<UserReadDTO>> GetAll();
        
        Task<UserReadDTO> GetById(int id);
        
        Task<string> CreateUser(UserCreateDTO dto);
      
        Task<bool> UpdateUser(int id, UserUpdateDTO dto);
        
        Task<bool> Delete(int id);
        
        
        
        Task<UserReadDTO> LoginAsync(string fullName, string password);
    }
}
