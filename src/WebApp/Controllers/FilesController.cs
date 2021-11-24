using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers;

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
    /// <remarks>
    /// Example find request:
    ///
    ///     POST /api/files
    ///     {
    ///       "path": "/mnt/azure/folder",
    ///       "filter": "*",
    ///       "recursive": true
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Find files and folders definition</param>
    /// <returns>Files and folders found</returns>
    /// <response code="200">Returns files and folders found</response>
    /// <response code="400">If request parameters are incorrectly defined</response>  
    /// <response code="500">If filesystem errors occur</response>  
    [HttpPost]
    [ProducesResponseType(typeof(GetFilesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        var time = Stopwatch.StartNew();
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

        response.Milliseconds = time.Elapsed.TotalMilliseconds;
        return new ObjectResult(response);
    }

    [HttpDelete]
    public GetFilesResponse Delete(GetFilesRequest request)
    {
        var time = Stopwatch.StartNew();
        var response = new GetFilesResponse()
        {
            Server = Environment.MachineName
        };

        Directory.Delete(request.Path, request.Recursive);

        response.Milliseconds = time.Elapsed.TotalMilliseconds;
        return response;
    }
}
