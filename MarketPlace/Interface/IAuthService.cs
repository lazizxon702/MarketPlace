using MarketPlace.DTO.Auth;

namespace MarketPlace.Interface;

public interface IAuthService
{
    Task<string> Login(AuthLoginDTO dto);
}