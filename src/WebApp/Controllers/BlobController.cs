using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Azure.Identity;
using Azure.Storage.Blobs.Specialized;
using System.Text;

namespace WebApp.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class BlobController : ControllerBase
{
    private readonly ILogger<BlobController> _logger;
    private readonly BlobServiceClient _blobServiceClient;

    public BlobController(ILogger<BlobController> logger, IConfiguration config)
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
    public async Task<ActionResult> Download(string container, string path, CancellationToken cancellationToken)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
        var blobClient = blobContainerClient.GetBlobClient(path);

        var blob = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return File(blob.Value.Content, blob.Value.Details.ContentType);
    }

    /// <summary>
    /// Upload blob from storage account.
    /// </summary>
    /// <param name="container">Container name</param>
    /// <param name="path">Blob path</param>
    /// <param name="currentChunk">Current chunk</param>
    /// <param name="totalChunks">Total chunks</param>
    /// <param name="data">Data stream</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Ok in successful upload</returns>
    /// <response code="200">Returns ok</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Upload(
        [FromForm] string container, [FromForm] string path, [FromForm] int currentChunk, [FromForm] int totalChunks,
        IFormFile data, CancellationToken cancellationToken)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
        var blobClient = blobContainerClient.GetBlockBlobClient(path);

        if (currentChunk == 1 && await blobClient.ExistsAsync(cancellationToken: cancellationToken))
        {
            return BadRequest("Blob already exists");
        }

        using var stream = data.OpenReadStream();
        var blockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(currentChunk.ToString("d6")));
        var blockBlob = await blobClient.StageBlockAsync(blockId, stream, cancellationToken: cancellationToken);

        if (currentChunk == totalChunks)
        {
            var blockIds = new List<string>();
            for (var i = 1; i <= totalChunks; i++)
            {
                blockIds.Add(Convert.ToBase64String(Encoding.UTF8.GetBytes(i.ToString("d6"))));
            }
            await blobClient.CommitBlockListAsync(blockIds, cancellationToken: cancellationToken);
        }
        return Ok();
    }
}
