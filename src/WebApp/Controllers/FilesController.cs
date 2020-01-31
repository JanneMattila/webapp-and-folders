using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
            var response = new GetFilesResponse()
            {
                Server = Environment.MachineName
            };

            foreach (var entry in entries)
            {
                response.Files.Add(entry);
            }
            return response;
        }

        [HttpDelete]
        public GetFilesResponse Delete(GetFilesRequest request)
        {
            var response = new GetFilesResponse()
            {
                Server = Environment.MachineName
            };

            Directory.Delete(request.Path, request.Recursive);
            return response;
        }
    }
}
