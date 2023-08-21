using MyBookListAPI.Models;

namespace MyBookListAPI.Dto
{
    public class BookResponse
    {
        public int Id { get; set; }
        public ICollection<string> Authors { get; set; } = new List<string>();
        public string GoogleBooksId { get; set; }
        public string Cover { get; set; }
        public Status Status { get; set; }
        public string Title { get; set; }
        public User User { get; set; }
    }
}
