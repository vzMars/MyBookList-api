using System.Text.Json.Serialization;

namespace MyBookListAPI.Dto
{
    public class VolumeInfo
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("authors")]
        public ICollection<string> Authors { get; set; } = new List<string>();

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("imageLinks")]
        public ImageLinks? ImageLinks { get; set; }
    }
}
