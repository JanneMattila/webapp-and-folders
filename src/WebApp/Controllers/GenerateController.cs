using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerateController : ControllerBase
    {
        private readonly ILogger<GenerateController> _logger;

        public GenerateController(ILogger<GenerateController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<CreateFilesResponse> Post(CreateFilesRequest request)
        {
            var response = new CreateFilesResponse
            {
                Path = request.Path,
                FilesCreated = await CreateStructure(request.Path, request.Folders, request.SubFolders, request.FilesPerFolder, request.FileSize)
            };
            return response;
        }

        private async Task<int> CreateStructure(string path, int folders, int subFolders, int filesPerFolder, int fileSize)
        {
            if (subFolders > 0)
            {
                var filesCreated = 0;
                for (int i = 1; i <= folders; i++)
                {
                    var childPath = Path.Combine(path, i.ToString());
                    Directory.CreateDirectory(childPath);
                    filesCreated += await CreateStructure(childPath, folders, subFolders - 1, filesPerFolder, fileSize);
                }
                return filesCreated;
            }
            else
            {
                var bytes = new byte[fileSize];
                for (int i = 1; i <= filesPerFolder; i++)
                {
                    var file = Path.Combine(path, $"{i}.txt");
                    await System.IO.File.WriteAllBytesAsync(file, bytes);
                }
                return filesPerFolder;
            }
        }
    }
}
