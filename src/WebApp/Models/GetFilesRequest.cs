﻿using System.Text.Json.Serialization;

namespace WebApp.Models;

public class GetFilesRequest
{
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("filter")]
    public string Filter { get; set; }

    [JsonPropertyName("recursive")]
    public bool Recursive { get; set; }
}
