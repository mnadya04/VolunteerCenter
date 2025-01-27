using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Events;
using VolunteerCenterMVCProject.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq.Expressions;
using VolunteerCenterMVCProject.ViewModels.Users;

namespace VolunteerCenterMVCProject.Controllers
{
	public class EventsController : Controller
	{
		private readonly IEventsService eventsService;
		private readonly ISignUpsService signUpsService;

		public EventsController(IEventsService eventsService, ISignUpsService signUpsService)
		{
			this.eventsService = eventsService;
			this.signUpsService = signUpsService;
		}



		// GET: Events/Index
		public async Task<IActionResult> Index(IndexEventsViewModel model)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



			Expression<Func<IndexEventViewModel, bool>> filter = i =>
			   (String.IsNullOrEmpty(model.Name) || i.Name.Contains(model.Name)) &&
			   (String.IsNullOrEmpty(model.Location) || i.Location.Contains(model.Location)) &&
			   (String.IsNullOrEmpty(model.Category) || i.Category.Contains(model.Category));


			model = await eventsService.GetEventsAsync(model,filter);

			return View(model);
		}


		// GET: Events/Details/{id}
		public async Task<IActionResult> Details(string id)
		{
			if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));

			try
			{
				var details = await eventsService.GetEventDetails(id);
				return View(details);
			}
			catch (KeyNotFoundException ex)
			{
				TempData["Error"] = ex.Message;
				return RedirectToAction(nameof(Index));
			}
		}

		[HttpGet]
		[Authorize(Roles = Constants.AdminRole)]
		public async Task<IActionResult> Create()
		{
			var model = new CreateEventViewModel
			{
				LocationOptions = await eventsService.PopulateLocationOptionsAsync(),
				CategoryOptions = await eventsService.PopulateCategoryOptionsAsync(),
				StatusOptions = eventsService.PopulateStatusOptions(),
				Deadline = DateTime.Now.AddDays(1)
			};

			return View(model);
		}


		[HttpPost]
		[Authorize(Roles = Constants.AdminRole)]
		public async Task<IActionResult> Create(CreateEventViewModel model)
		{
			if (!ModelState.IsValid)
			{
				// Repopulate dropdown options
				model.LocationOptions = await eventsService.PopulateLocationOptionsAsync(model.LocationId);
				model.CategoryOptions = await eventsService.PopulateCategoryOptionsAsync(model.CategoryId);
				model.StatusOptions = eventsService.PopulateStatusOptions();

				// Return the view with the updated model
				//    return View(model);
			}

			try
			{
				// Create the event using the service
				var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				model.CreatedBy = loggedUserId;
				await eventsService.CreateEventAsync(model);
				TempData["Success"] = "Event created successfully!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				TempData["Error"] = $"An error occurred: {ex.Message}";

				// Repopulate dropdown options on exception
				model.LocationOptions = await eventsService.PopulateLocationOptionsAsync(model.LocationId);
				model.CategoryOptions = await eventsService.PopulateCategoryOptionsAsync(model.CategoryId);
				model.StatusOptions = eventsService.PopulateStatusOptions();

				return View(model);
			}
		}




		// GET: Events/Edit
		public async Task<IActionResult> Edit(string id)
		{
			if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));

			try
			{
				var model = await eventsService.GetEventToEditAsync(id);

				// Populate dropdown options using the service
				model.LocationOptions = await eventsService.PopulateLocationOptionsAsync(model.LocationId);
				model.CategoryOptions = await eventsService.PopulateCategoryOptionsAsync(model.CategoryId);
				model.StatusOptions = eventsService.PopulateStatusOptions();

				return View(model);
			}
			catch (KeyNotFoundException ex)
			{
				TempData["Error"] = ex.Message;
				return RedirectToAction(nameof(Index));
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditEventViewModel model)
		{

			model.LocationOptions = await eventsService.PopulateLocationOptionsAsync(model.LocationId);
			model.CategoryOptions = await eventsService.PopulateCategoryOptionsAsync(model.CategoryId);
			model.StatusOptions = eventsService.PopulateStatusOptions();

			/*if (!ModelState.IsValid)
            {
                return View(model);
            }*/


			var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			model.ChangedBy = loggedUserId;
			await eventsService.EditEventByAdminAsync(model);
			TempData["Success"] = "Event updated successfully!";
			return RedirectToAction("Index");
		}




		[Authorize(Roles = Constants.AdminRole)]
		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			await this.eventsService.DeleteEventAsync(id);
			return RedirectToAction("Index");
		}


		public async Task<IActionResult> SignUpForEvent(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return RedirectToAction(nameof(Index));
			}
			var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var success = await signUpsService.SignUpForEventAsync(id, loggedUserId);

			return RedirectToAction("Index");
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CancelSignUp(string Id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			await signUpsService.CancelSignUpForEventAsync(Id, userId);

			return RedirectToAction("Index");
		}

	}
}
