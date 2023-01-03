using Newtonsoft.Json;

namespace WebAppDotnetFramework.Models
{
    public class CreateFilesRequest
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("folders")]
        public int Folders { get; set; }

        [JsonProperty("subFolders")]
        public int SubFolders { get; set; }

        [JsonProperty("filesPerFolder")]
        public int FilesPerFolder { get; set; }

        [JsonProperty("fileSize")]
        public int FileSize { get; set; }
    }
}
