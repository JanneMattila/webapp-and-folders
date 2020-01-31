using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;

        public FilesController(ILogger<FilesController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public GetFilesResponse Post(GetFilesRequest request)
        {
            var options = new EnumerationOptions()
            {
                RecurseSubdirectories = request.Recursive
            };
            
            var entries = Directory.EnumerateFileSystemEntries(request.Path, request.Filter, options);
            var response = new GetFilesResponse();
            foreach (var entry in entries)
            {
                response.Files.Add(entry);
            }
            return response;
        }

        [HttpDelete]
        public ActionResult Delete(GetFilesRequest request)
        {
            Directory.Delete(request.Path, request.Recursive);
            return Ok();
        }
    }
}
