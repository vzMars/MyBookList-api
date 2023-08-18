using MyBookListAPI.Dto;

namespace MyBookListAPI.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthResponse> Login(LoginRequest request);
        Task<AuthResponse> Logout();
        Task<AuthResponse> Signup(SignupRequest request);
    }
}
