using System.Text.Json.Serialization;

namespace MyBookListAPI.Dto
{
    public class SearchResult
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }

        [JsonPropertyName("items")]
        public ICollection<Volume>? Items { get; set; }
    }
}
