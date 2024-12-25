using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace VolunteerCenterMVCProject.Controllers
{

    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        //private readonly IEmailSender emailSender;
        //private readonly IUsersService service;

        public HomeController(ILogger<HomeController> logger)
        {
            //this.emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
