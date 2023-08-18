using Microsoft.AspNetCore.Identity;

namespace MyBookListAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Book> Books { get; set; }
        public ICollection<BookUser> BookUsers { get; set; }
    }
}
