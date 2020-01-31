using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class CreateFilesRequest
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
