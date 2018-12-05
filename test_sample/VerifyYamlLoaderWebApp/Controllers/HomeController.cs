using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VerifyYamlLoaderWebApp.Models;
using VerifyYamlLoaderWebApp.TypedOptions;

namespace VerifyYamlLoaderWebApp.Controllers
{
    public class HomeController : Controller
    {
#pragma warning disable IDE0044 // Add readonly modifier
        private MissionMainDataOption _missionMainData;
        private MissionRarityOption _missionRarityOption;
        private MissionTimeOption _missionTimeOption;
#pragma warning restore IDE0044 // Add readonly modifier

        public HomeController(
            IOptionsMonitor<MissionMainDataOption> missionMainDataOptionsMonitor,
            IOptionsMonitor<MissionRarityOption> missionRarityOptionsMonitor,
            IOptionsMonitor<MissionTimeOption> missionTimeOptionsMonitor)
        {
            _missionMainData = missionMainDataOptionsMonitor.CurrentValue;
            _missionRarityOption = missionRarityOptionsMonitor.CurrentValue;
            _missionTimeOption = missionTimeOptionsMonitor.CurrentValue;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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
