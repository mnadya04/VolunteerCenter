using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.ViewModels.Events;

namespace VolunteerCenterMVCProject.Services.Interfaces
{
	public interface IEventsService
	{
        Task CreateEventAsync(CreateEventViewModel model);

        Task DeleteEventAsync(string id);

        Task<IndexEventsViewModel> GetEventsAsync(IndexEventsViewModel model);

        Task<DetailsEventViewModel> GetEventDetails(string id);

        Task EditEventByAdminAsync(EditEventViewModel model);

        Task<EditEventViewModel> GetEventToEditAsync(string id);

        Task<string> GetUserId(string userId);

        Task<IEnumerable<LocationItem>> GetLocationsItemsAsync();

        Task<IEnumerable<CategoryItem>> GetCategoriesItemsAsync();

        Task<List<SelectListItem>> PopulateLocationOptionsAsync();

        Task<List<SelectListItem>> PopulateCategoryOptionsAsync();

        List<SelectListItem> PopulateStatusOptions();
    }
}

