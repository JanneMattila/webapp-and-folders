using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class CreateFilesResponse
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("filesCreated")]
        public int FilesCreated { get; set; }
    }
}
