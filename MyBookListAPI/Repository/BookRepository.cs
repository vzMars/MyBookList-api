using AutoMapper;
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
        private readonly IMapper _mapper;

        public BookRepository(HttpClient httpClient, IConfiguration config, ApplicationDbContext context, IMapper mapper)
        {
            _httpClient = httpClient;
            _config = config;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookUserResponse> AddBook(AddBookRequest request, string userId)
        {
            var response = new BookUserResponse();
            var book = new Book();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                response.Message = "User does not exist.";
                return response;
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
                response.Message = "Already added book to your list.";
                return response;
            }

            var bookUser = new BookUser
            {
                Book = book,
                User = user,
                Status = request.Status
            };

            await _context.BookUsers.AddAsync(bookUser);
            await _context.SaveChangesAsync();


            response.Book = new BookUserItem
            {
                Id = book.Id,
                Authors = request.Authors,
                GoogleBooksId = book.GoogleBooksId,
                Cover = book.Cover,
                Status = bookUser.Status,
                Title = book.Title,
                User = _mapper.Map<User>(user)
            };
            response.Success = true;
            return response;
        }

        public async Task<BookUserResponse> DeleteBook(string googleBooksId, string userId)
        {
            var response = new BookUserResponse();

            var bookUser = await _context.BookUsers
                .Include(bu => bu.User)
                .Include(bu => bu.Book)
                .ThenInclude(b => b.Authors)
                .FirstOrDefaultAsync(bu => bu.UserId == userId && bu.Book.GoogleBooksId == googleBooksId);

            if (bookUser == null)
            {
                response.Message = "Book has not been added to your list.";
                return response;
            }

            _context.BookUsers.Remove(bookUser);
            await _context.SaveChangesAsync();

            var authors = bookUser.Book.Authors.Select(a => a.Name).ToList();

            response.Book = new BookUserItem
            {
                Id = bookUser.Book.Id,
                Authors = authors,
                GoogleBooksId = bookUser.Book.GoogleBooksId,
                Cover = bookUser.Book.Cover,
                Status = bookUser.Status,
                Title = bookUser.Book.Title,
                User = _mapper.Map<User>(bookUser.User)
            };

            response.Success = true;
            return response;
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

        public async Task<ICollection<BookUserItem>> GetBooks()
        {
            var books = await _context.BookUsers
                .Select(bu => new BookUserItem
                {
                    Id = bu.BookId,
                    Authors = bu.Book.Authors.Select(a => a.Name).ToList(),
                    GoogleBooksId = bu.Book.GoogleBooksId,
                    Cover = bu.Book.Cover,
                    Status = bu.Status,
                    Title = bu.Book.Title,
                    User = _mapper.Map<User>(bu.User)
                })
                .ToListAsync();

            return books;
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

        public async Task<BookUserResponse> UpdateBookStatus(string googleBooksId, string userId, UpdateStatusRequest request)
        {
            var response = new BookUserResponse();

            var bookUser = await _context.BookUsers
                .Include(bu => bu.User)
                .Include(bu => bu.Book)
                .ThenInclude(b => b.Authors)
                .FirstOrDefaultAsync(bu => bu.UserId == userId && bu.Book.GoogleBooksId == googleBooksId);

            if (bookUser == null)
            {
                response.Message = "Book has not been added to your list.";
                return response;
            }

            bookUser.Status = request.Status;
            await _context.SaveChangesAsync();

            var authors = bookUser.Book.Authors.Select(a => a.Name).ToList();

            response.Book = new BookUserItem
            {
                Id = bookUser.Book.Id,
                Authors = authors,
                GoogleBooksId = bookUser.Book.GoogleBooksId,
                Cover = bookUser.Book.Cover,
                Status = bookUser.Status,
                Title = bookUser.Book.Title,
                User = _mapper.Map<User>(bookUser.User)
            };

            response.Success = true;
            return response;
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
