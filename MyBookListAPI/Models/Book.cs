namespace MyBookListAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string GoogleBooksId { get; set; }
        public string Title { get; set; }
        public string Cover { get; set; }
        public ICollection<Author> Authors { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<BookUser> BookUsers { get; set; }
    }
}
