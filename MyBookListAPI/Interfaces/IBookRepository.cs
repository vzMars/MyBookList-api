using MyBookListAPI.Dto;

namespace MyBookListAPI.Interfaces
{
    public interface IBookRepository
    {
        Task<ICollection<BookResponse>> GetBooks();
        Task<GetBookResponse> GetBook(string id, string userId);
        Task<ICollection<Volume>> SearchBooks(string query);
        Task<AddBookResponse> AddBook(AddBookRequest request, string userId);
    }
}
