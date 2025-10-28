using MarketPlace.DTO.Auth;
using MarketPlace.DTO.AuthDTO;

namespace MarketPlace.Interface;

public interface IAuthService
{
    Task<string> Login(AuthLoginDTO dto);
    Task<string> SignUp(AuthRegisterDTO dto);
}