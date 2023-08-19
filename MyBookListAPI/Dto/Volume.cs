using System.Text.Json.Serialization;

namespace MyBookListAPI.Dto
{
    public class Volume
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("volumeInfo")]
        public VolumeInfo VolumeInfo { get; set; }
    }
}
