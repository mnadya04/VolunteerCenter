using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerCenterMVCProject.Common;
using VolunteerCenterMVCProject.Services.Interfaces;

namespace VolunteerCenterMVCProject.Controllers
{
	public class DashboardController : Controller
	{

		[Authorize(Roles = Constants.AdminRole)]
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}
	}
}
