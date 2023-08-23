using MyBookListAPI.Dto;

namespace MyBookListAPI.Interfaces
{
    public interface IBookRepository
    {
        Task<ICollection<BookUserItem>> GetBooks();
        Task<GetBookResponse> GetBook(string id, string userId);
        Task<ICollection<Volume>> SearchBooks(string query);
        Task<BookUserResponse> AddBook(AddBookRequest request, string userId);
        Task<BookUserResponse> UpdateBookStatus(string googleBooksId, string userId, UpdateStatusRequest request);
    }
}
