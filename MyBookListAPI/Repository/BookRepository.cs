using Microsoft.EntityFrameworkCore;
using MyBookListAPI.Data;
using MyBookListAPI.Dto;
using MyBookListAPI.Interfaces;
using MyBookListAPI.Models;
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

        public async Task<AddBookResponse> AddBook(AddBookRequest request, string userId)
        {
            var addBookResponse = new AddBookResponse();
            var book = new Book();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                addBookResponse.Message = "User does not exist.";
                return addBookResponse;
            }

            var bookExists = await _context.Books.FirstOrDefaultAsync(b => b.GoogleBooksId == request.GoogleBooksId);
            if (bookExists == null)
            {
                var newBook = new Book
                {
                    GoogleBooksId = request.GoogleBooksId,
                    Title = request.Title,
                    Cover = request.Cover
                };

                await _context.Books.AddAsync(newBook);
                await AddAuthors(request.Authors, newBook);

                book = newBook;
            }
            else
            {
                book = bookExists;
            }

            var addedBookToList = await _context.BookUsers.FirstOrDefaultAsync(bu => bu.UserId == userId && bu.Book.GoogleBooksId == request.GoogleBooksId);
            if (addedBookToList != null)
            {
                addBookResponse.Message = "Already added book to your list.";
                return addBookResponse;
            }

            var bookUser = new BookUser
            {
                Book = book,
                User = user,
                Status = request.Status
            };

            await _context.BookUsers.AddAsync(bookUser);
            await _context.SaveChangesAsync();


            addBookResponse.Book = new BookResponse
            {
                Id = book.Id,
                Authors = book.Authors.Select(a => a.Name).ToList(),
                GoogleBooksId = book.GoogleBooksId,
                Cover = book.Cover,
                Status = bookUser.Status,
                Title = book.Title,
                User = new User
                {
                    Username = user.UserName!,
                    Id = user.Id
                }
            };
            addBookResponse.Success = true;
            return addBookResponse;
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

        private async Task AddAuthors(List<string> authors, Book book)
        {
            foreach (var author in authors)
            {
                var authorExists = await _context.Authors.FirstOrDefaultAsync(a => a.Name.ToLower() == author.ToLower());
                if (authorExists == null)
                {
                    var newAuthor = new Author
                    {
                        Name = author,
                    };

                    await _context.Authors.AddAsync(newAuthor);
                    await AddBookAuthor(newAuthor, book);
                }
                else
                {
                    await AddBookAuthor(authorExists, book);
                }
            }
        }

        private async Task AddBookAuthor(Author author, Book book)
        {
            var bookAuthor = new BookAuthor
            {
                Author = author,
                Book = book,
            };

            await _context.BookAuthors.AddAsync(bookAuthor);
        }
    }
}
