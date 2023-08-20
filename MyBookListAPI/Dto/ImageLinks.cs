using System.Text.Json.Serialization;

namespace MyBookListAPI.Dto
{
    public class ImageLinks
    {
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; } = string.Empty;

        [JsonPropertyName("smallThumbnail")]
        public string SmallThumbnail { get; set; } = string.Empty;
    }
}
