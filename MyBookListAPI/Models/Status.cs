using System.Text.Json.Serialization;

namespace MyBookListAPI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        Reading,
        Completed,
        Planning
    }
}
