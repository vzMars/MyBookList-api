namespace MyBookListAPI.Models
{
    public class BookUser
    {
        public int BookId { get; set; }
        public string UserId { get; set; }
        public Book Book { get; set; }
        public ApplicationUser User { get; set; }
        public Status Status { get; set; }
    }
}
