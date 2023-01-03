using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using WebAppDotnetFramework.Models;

namespace WebAppDotnetFramework.Controllers
{
    public class GenerateController : ApiController
    {
        public async Task<CreateFilesResponse> Post([FromBody] CreateFilesRequest request)
        {
            var time = Stopwatch.StartNew();
            var filesCreated = CreateStructure(request.Path, request.Folders, request.SubFolders, request.FilesPerFolder, request.FileSize);
            var response = new CreateFilesResponse
            {
                Path = request.Path,
                Server = Environment.MachineName,
                FilesCreated = filesCreated,
                Milliseconds = time.Elapsed.TotalMilliseconds
            };
            return response;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        private int CreateStructure(string path, int folders, int subFolders, int filesPerFolder, int fileSize)
        {
            if (subFolders > 0)
            {
                var filesCreated = 0;
                for (int i = 1; i <= folders; i++)
                {
                    var childPath = Path.Combine(path, i.ToString());
                    Directory.CreateDirectory(childPath);
                    filesCreated += CreateStructure(childPath, folders, subFolders - 1, filesPerFolder, fileSize);
                }
                return filesCreated;
            }
            else
            {
                var bytes = new byte[fileSize];
                for (int i = 1; i <= filesPerFolder; i++)
                {
                    var file = Path.Combine(path, $"{i}.txt");
                    System.IO.File.WriteAllBytes(file, bytes);
                }
                return filesPerFolder;
            }
        }
    }
}
