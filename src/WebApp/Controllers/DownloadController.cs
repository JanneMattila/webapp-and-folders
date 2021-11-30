using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class DownloadController : ControllerBase
{
    private readonly ILogger<DownloadController> _logger;

    public DownloadController(ILogger<DownloadController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Download file.
    /// </summary>
    /// <param name="request">Download file request</param>
    /// <returns>File based on the request definition</returns>
    /// <response code="200">Returns file based on request definition</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Post(DownloadRequest request)
    {
        return File(new byte[request.Size], "application/octet-stream");
    }
}
