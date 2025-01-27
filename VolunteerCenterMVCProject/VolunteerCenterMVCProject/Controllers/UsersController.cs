using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Linq.Expressions;
using System.Security.Claims;
using VolunteerCenterMVCProject.Common;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Shared;
using VolunteerCenterMVCProject.ViewModels.Users;

namespace VolunteerCenterMVCProject.Controllers
{

	public class UsersController : Controller
	{
		private readonly IUsersService service;

		public UsersController(IUsersService service)
		{
			this.service = service;
		}

		[HttpGet]
		[Authorize(Roles = Constants.AdminRole)]
		public async Task<IActionResult> Index(IndexVM model)
		{
			model.Pager ??= new PagerVM();

			model.Pager.Page = model.Pager.Page <= 0
										? 1
										: model.Pager.Page;

			model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0
										? 10
										: model.Pager.ItemsPerPage;

			string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			Expression<Func<UserVM, bool>> filter = i =>
				i.Id != loggedUserId &&
			   (String.IsNullOrEmpty(model.Email) || i.Email.Contains(model.Email)) &&
			   (String.IsNullOrEmpty(model.FirstName) || i.FirstName.Contains(model.FirstName)) &&
			   (String.IsNullOrEmpty(model.LastName) || i.LastName.Contains(model.LastName));

			model.Pager.PagesCount =
			  (int)Math.Ceiling(service.Count(filter) / (double)model.Pager.ItemsPerPage);

			IndexVM result = await service.GetAllAsync(filter, model.Pager.Page, model.Pager.ItemsPerPage, model.Pager.PagesCount);

			model.Users = result.Users;

			return View(model);
		}


		[HttpGet]
		[Authorize(Roles = Constants.AdminRole)]
		public IActionResult Create()
		{
			return View();
		}

		[Authorize(Roles = Constants.AdminRole)]
		[HttpPost]
		public async Task<IActionResult> Create(CreateVM model)
		{

			if (!ModelState.IsValid)
				return View(model);

			await service.CreateAsync(model);

			return RedirectToAction("Index");
		}


		[HttpGet]
		public async Task<IActionResult> Details(string id)
		{
			UserVM model = await service.GetUserAsync(id);

			if (model == null)
				return NotFound();

			return View(model);
		}
		[HttpGet]

		public async Task<IActionResult> Edit(string id)
		{
			var user = await service.GetUserAsync(id);

			if (user == null)
				return NotFound();


			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditUserVM model)
		{
			if (!ModelState.IsValid)
				return View(model);

			await service.UpdateAsync(model);

			var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (await service.IsInRoleAsync(loggedUserId, Constants.VolunteerRole))
				return RedirectToAction("Details", new { Id = loggedUserId });


			return RedirectToAction("Index");

		}


		[HttpGet]
		[Authorize(Roles = Constants.AdminRole)]

		public async Task<IActionResult> Delete(string id)
		{
			await service.DeleteAsync(id);
			return RedirectToAction("Index");
		}
	}
}
