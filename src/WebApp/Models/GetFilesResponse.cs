using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApp.Models;

public class GetFilesResponse
{
    [JsonPropertyName("server")]
    public string Server { get; set; }

    [JsonPropertyName("files")]
    public List<string> Files { get; set; }

    [JsonPropertyName("milliseconds")]
    public double Milliseconds { get; set; }

    public GetFilesResponse()
    {
        Files = new List<string>();
    }
}
