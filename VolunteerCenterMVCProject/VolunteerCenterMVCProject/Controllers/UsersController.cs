using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Linq.Expressions;
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
		private readonly IUsersService service;

		public UsersController(IUsersService userService)
		{
			this.service = userService;
		}

		[HttpGet]
		public async Task<IActionResult> Index(IndexVM model)
		{ 
			model.Pager ??= new PagerVM();

			model.Pager.Page = model.Pager.Page <= 0
										? 1
										: model.Pager.Page;

			model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0
										? 10
										: model.Pager.ItemsPerPage;

			Expression<Func<UserVM, bool>> filter = i =>
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
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
        public async Task<IActionResult> Create(CreateUserVM model)
		{

			if (!ModelState.IsValid)
				return View(model);

			await service.CreateAsync(model);

			return RedirectToAction("Index");
        }

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var user = await service.EditAsync(id);

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

			return RedirectToAction("Index");
			
		}


        [HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			await service.DeleteAsync(id);
			return RedirectToAction("Index");
		}
	}
}
