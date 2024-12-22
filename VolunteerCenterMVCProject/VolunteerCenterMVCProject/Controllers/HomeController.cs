using Microsoft.AspNetCore.Mvc;

namespace VolunteerCenterMVCProject.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
