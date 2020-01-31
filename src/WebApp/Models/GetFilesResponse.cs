using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class GetFilesResponse
    {
        [JsonPropertyName("files")]
        public List<FileModel> Files { get; set; }

        public GetFilesResponse()
        {
            Files = new List<FileModel>();
        }
    }
}
