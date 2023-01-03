using Newtonsoft.Json;

namespace WebAppDotnetFramework.Models
{
    public class CreateFilesResponse
    {
        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("filesCreated")]
        public int FilesCreated { get; set; }

        [JsonProperty("milliseconds")]
        public double Milliseconds { get; set; }
    }
}
