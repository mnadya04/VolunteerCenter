using System.Linq.Expressions;
using VolunteerCenterMVCProject.ViewModels.Locations;

namespace VolunteerCenterMVCProject.Services.Interfaces
{
	public interface ILocationService
	{

		Task CreateAsync(CreateVM model);
		Task<LocationVM> GetLocationAsync(string id);
		Task UpdateAsync(EditVM model);

		Task DeleteAsync(string id);
		Task<IndexVM> GetAllAsync(Expression<Func<LocationVM, bool>> filter);

	}
}