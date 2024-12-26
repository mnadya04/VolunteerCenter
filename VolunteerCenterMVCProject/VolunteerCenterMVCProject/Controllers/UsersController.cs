using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using VolunteerCenterMVCProject.Common;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Shared;
using VolunteerCenterMVCProject.ViewModels.Users;

namespace VolunteerCenterMVCProject.Controllers
{

	[Authorize(Roles = Constants.AdminRole)]
	public class UsersController : Controller
	{
		private readonly IUsersService usersService;

		public UsersController(IUsersService userService)
		{
			this.usersService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> Index(UsersVM model)
		{
			model.Pager ??= new PagerVM();


			model.Pager ??= new PagerVM();

			model.Pager.Page = model.Pager.Page <= 0
										? 1
										: model.Pager.Page;

			model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0
										? 10
										: model.Pager.ItemsPerPage;

			int c = usersService.Count();
			model.Pager.PagesCount =
			  (int)Math.Ceiling(usersService.Count() / (double)model.Pager.ItemsPerPage);

			var result = await usersService.GetUsersAsync(model.Pager.Page, model.Pager.ItemsPerPage, model.Pager.PagesCount);

			model.Users = result.Users;

			return View(model);
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

			return RedirectToAction("Index");
        }

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var user = await usersService.GetUserEditByIdAsync(id);

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
