using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;

        public FilesController(ILogger<FilesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Find files and folders from filesystem.
        /// </summary>
        /// <param name="request">Find files and folders definition</param>
        /// <returns>Files and folders found</returns>
        /// <response code="200">Returns files and folders found</response>
        /// <response code="400">If request parameters are incorrectly defined</response>  
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ObjectResult Post(GetFilesRequest request)
        {
            if (request is null || request.Path is null)
            {
                var problemDetail = new ProblemDetails
                {
                    Status = 400,
                    Title = "Invalid request",
                    Detail = "Request",
                    Instance = "https://api.contoso.com/errors/400"
                };

                return new ObjectResult(problemDetail)
                {
                    StatusCode = problemDetail.Status
                };
            }

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
            return new ObjectResult(response);
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
