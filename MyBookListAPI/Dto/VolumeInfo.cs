using System.Text.Json.Serialization;

namespace MyBookListAPI.Dto
{
    public class VolumeInfo
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("authors")]
        public ICollection<string>? Authors { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
