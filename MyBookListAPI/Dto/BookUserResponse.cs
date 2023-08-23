namespace MyBookListAPI.Dto
{
    public class BookUserResponse
    {
        public BookUserItem? Book { get; set; }
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
