using MyBookListAPI.Models;

namespace MyBookListAPI.Dto
{
    public class GetBookResponse
    {
        public VolumeInfo? Details { get; set; }
        public Status? Status { get; set; }
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;

    }
}
