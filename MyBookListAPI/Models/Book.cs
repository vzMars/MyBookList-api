namespace MyBookListAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Cover { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
