using Microsoft.AspNetCore.Mvc;

namespace VolunteerCenterProject.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
