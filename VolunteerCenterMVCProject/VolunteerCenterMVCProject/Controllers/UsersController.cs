using Microsoft.AspNetCore.Mvc;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Users;

namespace VolunteerCenterMVCProject.Controllers
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVM model)
		{

			if (!ModelState.IsValid)
				return View(model);

			await usersService.CreateUserAsync(model);

			//return RedirectToPage("/Account/Login", new { area = "Identity" });
			return RedirectToAction("Index");
        }

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var user = await usersService.GetUserByIdAsync(id);

			if (user == null)
				return NotFound();


			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditUserVM model)
		{
			if (!ModelState.IsValid)
				return View(model);

			await usersService.UpdateUserAsync(model);

			return RedirectToAction("Index");
			
		}


        [HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			await usersService.DeleteUserByIdAsync(id);
			return RedirectToAction("Index");
		}
	}
}
