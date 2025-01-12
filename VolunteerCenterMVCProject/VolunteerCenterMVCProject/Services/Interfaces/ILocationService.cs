using System.Linq.Expressions;
using VolunteerCenterMVCProject.ViewModels.Locations;

namespace VolunteerCenterMVCProject.Services.Interfaces
{
	public interface ILocationService
	{
		int Count();

		Task CreateAsync(CreateVM model);
		Task<LocationVM> GetLocationAsync(string id);
		//Task<EditVM> GetEditLocationAsync(string id);
		Task UpdateAsync(EditVM model);

		Task DeleteAsync(string id);
		Task<IndexVM> GetAllAsync();

	}
}