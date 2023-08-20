using MyBookListAPI.Dto;

namespace MyBookListAPI.Interfaces
{
    public interface IBookRepository
    {
        Task<ICollection<Volume>> SearchBooks(string query);
    }
}
