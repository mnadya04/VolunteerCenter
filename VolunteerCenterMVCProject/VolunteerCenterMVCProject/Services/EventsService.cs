using System;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using VolunteerCenterMVCProject.Data;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Categories;
using VolunteerCenterMVCProject.ViewModels.Events;
using VolunteerCenterMVCProject.ViewModels.Shared;
using VolunteerCenterMVCProject.ViewModels.Users;
using Location = VolunteerCenterMVCProject.Models.Location;

namespace VolunteerCenterMVCProject.Services
{
	public class EventsService : IEventsService
	{
		private readonly ApplicationDbContext context;

		public EventsService(ApplicationDbContext context)
		{
			this.context = context;
		}

		public async Task CreateEventAsync(CreateEventViewModel model)
		{
			// Retrieve location and category based on the selected values
			Location? location = await this.context.Locations
				.FirstOrDefaultAsync(l => l.LocationId == model.LocationId); // Match LocationId, not LocationCity

			Category? category = await this.context.Categories
				.FirstOrDefaultAsync(c => c.CategoryId == model.CategoryId); // Match CategoryId, not CategoryName

			// Throw exception if location or category is invalid
			if (location == null) throw new Exception("Invalid Location.");
			if (category == null) throw new Exception("Invalid Category.");

			// Create the event
			Event newEvent = new Event
			{
				CreatedBy = model.CreatedBy,
				Name = model.Name,
				Description = model.Description,
				Deadline = model.Deadline,
				Budget = model.Budget,
				LocationId = location.LocationId,  // Use the LocationId from the dropdown
				CategoryId = category.CategoryId,
				Status = model.Status ?? "Waiting"  // Set the default status if not provided
			};

			// Add event to the context and save changes
			await context.Events.AddAsync(newEvent);

			var statusHistory = new StatusHistory
			{
				EventId = newEvent.EventId,
				ChangedBy = model.CreatedBy,
				NewStatus = newEvent.Status,
				ChangeDate = DateTime.Now
			};
			await context.statusHistories.AddAsync(statusHistory);

			await context.SaveChangesAsync();
		}

		public async Task EditEventByAdminAsync(EditEventViewModel model)
		{
			// Find the event by its ID
			Event? eventToEdit = await this.context.Events.FirstOrDefaultAsync(e => e.EventId == model.Id);
			if (eventToEdit == null) throw new KeyNotFoundException("Event not found.");

			// Find the related location and category by their IDs
			Location? location = await this.context.Locations.FirstOrDefaultAsync(l => l.LocationId == model.LocationId);
			Category? category = await this.context.Categories.FirstOrDefaultAsync(c => c.CategoryId == model.CategoryId);

			if (location == null) throw new Exception("Invalid Location.");
			if (category == null) throw new Exception("Invalid Category.");

			// Update the event's basic properties
			eventToEdit.Name = model.Name;
			eventToEdit.Description = model.Description;
			eventToEdit.Deadline = model.Deadline;
			eventToEdit.Budget = model.Budget;
			eventToEdit.LocationId = location.LocationId;
			eventToEdit.CategoryId = category.CategoryId;

			// Check if the status has changed
			if (eventToEdit.Status != model.Status)
			{
				string userId = model.ChangedBy;
				;
				var statusHistory = new StatusHistory
				{
					EventId = model.Id,
					ChangedBy = userId,      // Set the ID of the user making the change
					NewStatus = model.Status,
					ChangeDate = DateTime.Now
				};

				await context.statusHistories.AddAsync(statusHistory);

				eventToEdit.Status = model.Status;  // Update the status
			}

			// Save changes to the database
			await context.SaveChangesAsync();
		}


		public async Task<IEnumerable<LocationItem>> GetLocationsItemsAsync()
		{
			return await context.Locations
				.Select(location => new LocationItem
				{
					LocationId = location.LocationId,
					City = location.City
				})
				.ToListAsync();
		}

		public async Task<IEnumerable<CategoryItem>> GetCategoriesItemsAsync()
		{
			return await context.Categories
				.Select(category => new CategoryItem
				{
					CategoryId = category.CategoryId,
					Name = category.Name
				})
				.ToListAsync();
		}

		// New methods for populating dropdown options:

		public async Task<List<SelectListItem>> PopulateLocationOptionsAsync(string selectedLocationId = null)
		{
			var locations = await GetLocationsItemsAsync();
			return locations.Select(loc => new SelectListItem
			{
				Value = loc.LocationId,
				Text = loc.City,
				Selected = loc.LocationId == selectedLocationId
			}).ToList();
		}

		public async Task<List<SelectListItem>> PopulateCategoryOptionsAsync(string selectedCategoryId = null)
		{
			var categories = await GetCategoriesItemsAsync();
			return categories.Select(cat => new SelectListItem
			{
				Value = cat.CategoryId,
				Text = cat.Name,
				Selected = cat.CategoryId == selectedCategoryId
			}).ToList();
		}

		public List<SelectListItem> PopulateStatusOptions()
		{
			var statusOptions = new List<SelectListItem>
		{
			new SelectListItem { Value = "Waiting", Text = "Waiting" },
			new SelectListItem { Value = "Assigned", Text = "Assigned" },
			new SelectListItem { Value = "In Progress", Text = "In Progress" },
			new SelectListItem { Value = "Completed", Text = "Completed" },
			new SelectListItem { Value = "Canceled", Text = "Canceled" }
		};

			return statusOptions;
		}


		public async Task DeleteEventAsync(string id)
		{
			Event eventToRemove = await this.context.Events.FindAsync(id);

			if (eventToRemove == null)
			{
				throw new KeyNotFoundException("Event not found.");
			}

			var statusHistoryEntries = this.context.statusHistories
			.Where(sh => sh.EventId == id)
			.ToList();
			this.context.statusHistories.RemoveRange(statusHistoryEntries);

			var signUpsEntries = this.context.Signups
			.Where(s => s.EventId == id)
			.ToList();
			this.context.Signups.RemoveRange(signUpsEntries);

			// Remove the event
			this.context.Events.Remove(eventToRemove);

			// Save changes to the database
			await this.context.SaveChangesAsync();
		}

		public async Task<DetailsEventViewModel> GetEventDetails(string id)
		{
			return await this.context.Events
				.Where(e => e.EventId == id)
				.Select(e => new DetailsEventViewModel
				{
					Name = e.Name,
					Description = e.Description,
					Deadline = e.Deadline.ToString("dddd, dd MMMM yyyy"),
					Budget = e.Budget,
					Status = e.Status,
					User = string.Concat(e.User.FirstName, " ", e.User.LastName),
					Location = e.Location.City,
					Category = e.Category.Name
				})
				.FirstOrDefaultAsync();
		}


		public async Task<IndexEventsViewModel> GetEventsAsync(IndexEventsViewModel model,
	   Expression<Func<IndexEventViewModel, bool>> filter = null)
		{
			model.Pager ??= new PagerVM { Page = 1, ItemsPerPage = 10 };

			var query = this.context.Events
				.Include(e => e.Location)
				.Include(e => e.Category)
				.Include(e => e.User)
				.OrderBy(e => e.Name)
				.Select(e => new IndexEventViewModel
				{
					Id = e.EventId,
					Name = e.Name,
					Location = e.Location.City,
					Category = e.Category.Name,
					Deadline = e.Deadline,
					Budget = e.Budget,
					Status = e.Status,
					CreatedBy = $"{e.User.FirstName} {e.User.LastName}"
				});

			if (filter != null)
			{
				query = query.Where(filter);
			}

			int totalEvents = await query.CountAsync();

			model.Pager.PagesCount = (int)Math.Ceiling(totalEvents / (double)model.Pager.ItemsPerPage);

			model.Events = await query
				.Skip((model.Pager.Page - 1) * model.Pager.ItemsPerPage)
				.Take(model.Pager.ItemsPerPage)
				.ToListAsync();

			return model;
		}

		public async Task<EditEventViewModel> GetEventToEditAsync(string id)
		{
			Event? eventToEdit = await this.context.Events
				.Include(e => e.Location)
				.Include(e => e.Category)
				.FirstOrDefaultAsync(e => e.EventId == id);

			if (eventToEdit == null)
			{
				throw new KeyNotFoundException("Event not found.");
			}

			EditEventViewModel model = new EditEventViewModel
			{
				Id = eventToEdit.EventId,
				Name = eventToEdit.Name,
				Description = eventToEdit.Description,
				Deadline = eventToEdit.Deadline,
				Budget = eventToEdit.Budget,
				LocationCity = eventToEdit.Location.City,
				CategoryName = eventToEdit.Category.Name,
				Status = eventToEdit.Status,
				ChangedBy = eventToEdit.CreatedBy,
				LocationId = eventToEdit.LocationId,
				CategoryId = eventToEdit.CategoryId
			};

			return model;
		}



		public async Task<string> GetUserId(string userId)
		{
			var result = await this.context.Users.FirstOrDefaultAsync(x => x.Id == userId);
			return result.Id;
		}
	}
}

