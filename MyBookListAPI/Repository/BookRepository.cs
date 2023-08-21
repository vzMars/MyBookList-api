using Microsoft.EntityFrameworkCore;
using MyBookListAPI.Data;
using MyBookListAPI.Dto;
using MyBookListAPI.Interfaces;
using System.Text.Json;

namespace MyBookListAPI.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public BookRepository(HttpClient httpClient, IConfiguration config, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _config = config;
            _context = context;
        }

        public async Task<GetBookResponse> GetBook(string id, string userId)
        {
            var getBookResponse = new GetBookResponse();
            string url = $"https://books.googleapis.com/books/v1/volumes/{id}?key={_config["GoogleBooks:ServiceApiKey"]}";
            var response = await _httpClient.GetAsync(url);

            try
            {
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Volume>(json)!;

                var bookUser = await _context.BookUsers.FirstOrDefaultAsync(u => u.UserId == userId && u.Book.GoogleBooksId == result.Id);

                if (bookUser != null)
                {
                    getBookResponse.Status = bookUser.Status;
                }

                getBookResponse.Details = result.VolumeInfo;
                getBookResponse.Success = true;

                return getBookResponse;
            }
            catch
            {
                getBookResponse.Message = "The book ID could not be found.";
                return getBookResponse;
            }
        }

        public async Task<ICollection<Volume>> SearchBooks(string query)
        {
            string searchQuery = query.Replace(" ", "%20");
            string url = $"https://books.googleapis.com/books/v1/volumes?q={searchQuery}&maxResults=40&key={_config["GoogleBooks:ServiceApiKey"]}";

            var response = await _httpClient.GetAsync(url);
            try
            {
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<SearchResult>(json)!;

                if (result.Items == null)
                    throw new Exception();

                return result.Items;
            }
            catch
            {
                return Array.Empty<Volume>();
            }
        }
    }
}
