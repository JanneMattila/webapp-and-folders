using Microsoft.AspNetCore.Mvc;
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
    [ProducesResponseType(typeof(UploadResponse), StatusCodes.Status200OK)]
    public ActionResult Post()
    {
        return new OkObjectResult(new UploadResponse()
        {
            Size = Request.ContentLength.Value
        });
    }
}
