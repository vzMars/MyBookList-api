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

        public async Task<ICollection<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return _mapper.Map<List<User>>(users);
        }
    }
}
