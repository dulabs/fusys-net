using System;
using System.Linq;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fu.Module.Core.Controllers;

namespace Fu.Module.Localization.Controllers
{
    [Route("translate")]
    public class LocalizationController : PublicController
    {
        private readonly ILogger _logger;

        public LocalizationController(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger("Unhandled Error");
        }

        public IActionResult Index()
        {
            return Content("Test Localization");
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            return Content("Add Translate");
        }

    }
}