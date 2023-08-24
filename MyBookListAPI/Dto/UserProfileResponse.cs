namespace MyBookListAPI.Dto
{
    public class UserProfileResponse
    {
        public ICollection<BookUserItem> Books { get; set; } = new List<BookUserItem>();
        public string Username { get; set; }
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
