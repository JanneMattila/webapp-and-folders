﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;

        public FilesController(ILogger<FilesController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public GetFilesResponse Get(GetFilesRequest request)
        {
            var response = new GetFilesResponse();
            response.Files.Add(new FileModel() { Name = "placeholder.txt" });
            return response;
        }
    }
}