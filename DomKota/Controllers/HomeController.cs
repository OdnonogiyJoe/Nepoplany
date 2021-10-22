using DNTCaptcha.Core;
using DomKota.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DomKota.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly DNTCaptchaOptions _catchaOptions;
        public HomeController(ILogger<HomeController> logger, IDNTCaptchaValidatorService validatorService, IOptions<DNTCaptchaOptions> catchaOptions)
        {
            _logger = logger;
            _validatorService = validatorService;
            _catchaOptions = catchaOptions==null?throw new ArgumentNullException(nameof(catchaOptions)):catchaOptions.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Login(LoginViewModel LoginViewModel)
        {
            if(ModelState.IsValid)
            {
                if(!_validatorService.HasRequestValidCaptchaEntry(Language.English,DisplayMode.NumberToWord))//Тут меняется задание для входа
                {
                    this.ModelState.AddModelError(_catchaOptions.CaptchaComponent.CaptchaInputName, "Please enter the security code as number.");
                    return View("Index");
                }
            }

            return View("Dashboard");
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
