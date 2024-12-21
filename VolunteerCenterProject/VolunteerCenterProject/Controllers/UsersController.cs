using Microsoft.AspNetCore.Mvc;
using VolunteerCenterProject.Models;
using VolunteerCenterProject.Services;
using VolunteerCenterProject.Services.Interfaces;
using VolunteerCenterProject.ViewModels.Users;

namespace VolunteerCenterProject.Controllers
{


	public class UsersController : Controller
	{
		private readonly IUsersService usersService;

		public UsersController(IUsersService userService)
		{
			this.usersService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var items = await usersService.GetUsersAsync();
			return View(items);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(CreateUserVM model)
		{

			if (!ModelState.IsValid)
				return View(model);

			usersService.CreateAsync(model);

			return RedirectToAction("Index");

		}


	}
}
