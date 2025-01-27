using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Events;
using VolunteerCenterMVCProject.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace VolunteerCenterMVCProject.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventsService eventsService;

        public EventsController(IEventsService eventsService)
        {
            this.eventsService = eventsService;
        }

        // GET: Events/Index
        public async Task<IActionResult> Index(IndexEventsViewModel model)
        {
            model = await eventsService.GetEventsAsync(model);
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
                Deadline=DateTime.Now.AddDays(1)
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> Create(CreateEventViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    // Repopulate dropdown options
            //    model.LocationOptions = await eventsService.PopulateLocationOptionsAsync();
            //    model.CategoryOptions = await eventsService.PopulateCategoryOptionsAsync();
            //    model.StatusOptions = eventsService.PopulateStatusOptions();
            //
            //    // Return the view with the updated model
            //    return View(model);
            //}

            try
            {
                // Create the event using the service
                await eventsService.CreateEventAsync(model);
                TempData["Success"] = "Event created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";

                // Repopulate dropdown options on exception
                model.LocationOptions = await eventsService.PopulateLocationOptionsAsync();
                model.CategoryOptions = await eventsService.PopulateCategoryOptionsAsync();
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
                model.LocationOptions = await eventsService.PopulateLocationOptionsAsync();
                model.CategoryOptions = await eventsService.PopulateCategoryOptionsAsync();
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdown options if validation fails
                model.LocationOptions = await eventsService.PopulateLocationOptionsAsync();
                model.CategoryOptions = await eventsService.PopulateCategoryOptionsAsync();
                model.StatusOptions = eventsService.PopulateStatusOptions(); 
                return View(model);
            }

            try
            {
                await eventsService.EditEventByAdminAsync(model);
                TempData["Success"] = "Event updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                // Repopulate dropdown options on exception
                model.LocationOptions = await eventsService.PopulateLocationOptionsAsync();
                model.CategoryOptions = await eventsService.PopulateCategoryOptionsAsync();
                model.StatusOptions = eventsService.PopulateStatusOptions(); 
                return View(model);
            }
        }

        // Delete action (if applicable, needs to confirm action before proceeding)
        [Authorize(Roles = Constants.AdminRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));

            try
            {
                // Confirm deletion here if needed, for example
                await eventsService.DeleteEventAsync(id);
                TempData["Success"] = "Event deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        
    }
}
