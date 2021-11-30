using System.Text.Json.Serialization;

namespace WebApp.Models;

public class DownloadRequest
{
    [JsonPropertyName("size")]
    public long Size { get; set; }
}
