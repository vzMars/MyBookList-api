using MyBookListAPI.Dto;

namespace MyBookListAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers();
    }
}
