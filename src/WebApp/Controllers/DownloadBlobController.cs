using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Azure.Identity;

namespace WebApp.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class DownloadBlobController : ControllerBase
{
    private readonly ILogger<DownloadBlobController> _logger;
    private readonly BlobServiceClient _blobServiceClient;

    public DownloadBlobController(ILogger<DownloadBlobController> logger, IConfiguration config)
    {
        _logger = logger;

        var blobUri = config["BlobUri"];
        _blobServiceClient = new BlobServiceClient(new Uri(blobUri), new DefaultAzureCredential());
    }

    /// <summary>
    /// Download blob from storage account.
    /// </summary>
    /// <param name="container">Container name</param>
    /// <param name="path">Blob path</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>File based on the request definition</returns>
    /// <response code="200">Returns file stream based on container name and path</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Get(string container, string path, CancellationToken cancellationToken)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
        var blobClient = blobContainerClient.GetBlobClient(path);

        var blob = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return File(blob.Value.Content, blob.Value.Details.ContentType);
    }
}
