namespace MyBookListAPI.Dto
{
    public class AuthResponse
    {
        public AuthUser? User { get; set; }
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
