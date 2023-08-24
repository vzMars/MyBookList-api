using MyBookListAPI.Dto;

namespace MyBookListAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUsers();
        Task<UserProfileResponse> GetUserProfile(string username);
    }
}
