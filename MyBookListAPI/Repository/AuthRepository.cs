using MyBookListAPI.Dto;
using MyBookListAPI.Interfaces;

namespace MyBookListAPI.Repository
{
    public class AuthRepository : IAuthRepository
    {
        public Task<AuthResponse> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponse> Logout()
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponse> Signup(SignupRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
