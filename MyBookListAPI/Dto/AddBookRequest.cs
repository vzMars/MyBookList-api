using MyBookListAPI.Models;

namespace MyBookListAPI.Dto
{
    public class AddBookRequest
    {
        public string GoogleBooksId { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Cover { get; set; }
        public Status Status { get; set; }
    }
}
