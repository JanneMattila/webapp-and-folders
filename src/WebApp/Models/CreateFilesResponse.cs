using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class CreateFilesResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
