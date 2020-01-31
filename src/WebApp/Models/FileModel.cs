using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class FileModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
