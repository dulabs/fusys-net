using System;
using System.Linq;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Fu.Module.Core.Models;
using Fu.Infrastructure.Data;

namespace Fu.Module.Core.Controllers
{
    public class HomeController : PublicController
    {
        private readonly ILogger _logger;
        private readonly IRepository<User> _users;

        public HomeController(ILoggerFactory factory, IRepository<User> users)
        {
            _logger = factory.CreateLogger("Unhandled Error");
            _users = users;
        }

        public IActionResult TestError()
        {
            throw new Exception("Test behavior in case of error");
        }

        [HttpGet("/Home/ErrorWithCode/{statusCode}")]
        public IActionResult ErrorWithCode(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("404");
            }

            return View("Error");
        }

        public IActionResult Error()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;

            if (error != null)
            {
                _logger.LogError(error.Message, error);
            }

            return View("Error");
        }

    }
}