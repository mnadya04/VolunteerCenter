using Microsoft.AspNetCore.Mvc.Rendering;
using VolunteerCenterMVCProject.ViewModels.Events;

namespace VolunteerCenterMVCProject.Services.Interfaces
{
	public interface IEventsService
	{
		Task CreateAsync(CreateEventVM model);

		Task<EventVM> GetEventByIdAsync(string id);
		Task<EventsVM> GetEventsAsync(int page = 1, int count = 10);
		Task DeleteEventByIdAsync(string id);
		//Task UpdateEventAsync(EditUserVM model);
		Task<SelectList> GetAllUsersAsync();
	}
}
