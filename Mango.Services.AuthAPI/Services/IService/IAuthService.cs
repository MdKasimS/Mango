using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI.Services.IService
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegistrationRequestDto registrationRequestDto);
        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
