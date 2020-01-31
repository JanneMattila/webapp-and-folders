using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public CreateFilesResponse Post(CreateFilesRequest request)
        {
            var response = new CreateFilesResponse();
            return response;
        }
    }
}
