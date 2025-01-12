using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerCenterMVCProject.Common;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Locations;

namespace VolunteerCenterMVCProject.Controllers
{

	[Authorize(Roles = Constants.AdminRole)]
	public class LocationsController : Controller
	{

		private readonly ILocationService service;

		public LocationsController(ILocationService service)
		{
			this.service = service;
		}

		[HttpGet]
		public async Task<IActionResult> Index(IndexVM model)
		{
			IndexVM result = await service.GetAllAsync();
			model.Locations = result.Locations;
			
			return View(model);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateVM model)
		{
			if(!ModelState.IsValid)
				return View(model);

			await service.CreateAsync(model);

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{

			var model = await service.GetLocationAsync(id);

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditVM model)
		{
			if(!ModelState.IsValid)
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
