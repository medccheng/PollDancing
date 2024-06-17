using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Data;
using PollDancingLibrary.Models;
using PollDancingWeb.Models;
using System.Diagnostics;
using NLog;

namespace PollDancingWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CongressDbContext _congressDbContext;
        public HomeController(ILogger<HomeController> logger, CongressDbContext dbContext)
        {
            _logger = logger;
            _congressDbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

    

        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
