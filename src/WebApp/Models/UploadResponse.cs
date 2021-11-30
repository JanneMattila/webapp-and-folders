using System.Text.Json.Serialization;

namespace WebApp.Models;

public class UploadResponse
{
    [JsonPropertyName("size")]
    public long Size { get; set; }
}
