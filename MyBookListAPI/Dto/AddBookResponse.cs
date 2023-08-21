namespace MyBookListAPI.Dto
{
    public class AddBookResponse
    {
        public BookResponse? Book { get; set; }
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
