using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBookListAPI.Data;
using MyBookListAPI.Dto;
using MyBookListAPI.Interfaces;

namespace MyBookListAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserProfileResponse> GetUserProfile(string username)
        {
            var response = new UserProfileResponse();
            var user = _context.Users
                .Include(u => u.BookUsers)
                .Include(u => u.Books)
                .ThenInclude(b => b.Authors)
                .FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                response.Message = "Profile not found.";
                return response;
            }

            var books = user.BookUsers.Select(bu =>
                new BookUserItem
                {
                    Id = bu.Book.Id,
                    Authors = bu.Book.Authors.Select(a => a.Name).ToList(),
                    GoogleBooksId = bu.Book.GoogleBooksId,
                    Cover = bu.Book.Cover,
                    Status = bu.Status,
                    Title = bu.Book.Title,
                    User = _mapper.Map<User>(bu.User)
                }
            ).ToList();

            response.Books = books;
            response.Username = user.UserName!;
            response.Success = true;
            return response;
        }

        public async Task<ICollection<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return _mapper.Map<List<User>>(users);
        }
    }
}
