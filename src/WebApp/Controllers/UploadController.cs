using Microsoft.AspNetCore.Mvc;
using System.Buffers;
using WebApp.Models;

namespace WebApp.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly ILogger<UploadController> _logger;

    public UploadController(ILogger<UploadController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Upload file.
    /// </summary>
    /// <returns>Number of bytes uploaded</returns>
    /// <response code="200">Returns number of bytes uploaded</response>
    [HttpPost]
    [RequestSizeLimit(int.MaxValue)]
    [ProducesResponseType(typeof(UploadResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult> Post()
    {
        if (!Request.ContentLength.HasValue)
        {
            return new BadRequestObjectResult("Content-Length is required header.");
        }

        var readBytes = 0;
        var buffer = ArrayPool<byte>.Shared.Rent(4096);
        while (true)
        {
            var read = await Request.Body.ReadAsync(buffer, 0, buffer.Length);
            if (read == 0)
            {
                break;
            }
            readBytes += read;
        }
        
        ArrayPool<byte>.Shared.Return(buffer);
        return new OkObjectResult(new UploadResponse()
        {
            Size = readBytes
        });
    }
}
