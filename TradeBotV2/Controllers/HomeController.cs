using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using TradeBot.Services;
using TradeBotV2.Models;

namespace TradeBotV2.Controllers
{
    public class HomeController : Controller
{
        private readonly ILogger<HomeController> _logger;
        private readonly GhasedakSMSProvider _smsProvider;
        private readonly timerTest _timertest;

        public HomeController(ILogger<HomeController> logger , GhasedakSMSProvider ghasedakSmsProvider , timerTest timertest)
        {
            _logger = logger;
            _smsProvider = ghasedakSmsProvider;
            _timertest = timertest;
        }

        public IActionResult Index()
        {
            //Example.Starter(_timertest, _smsProvider);
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