using System.Linq.Expressions;
using VolunteerCenterMVCProject.ViewModels.StatusHistory;

namespace VolunteerCenterMVCProject.Services.Interfaces
{
	public interface IStatusHistoryService
	{
		int Count();
		Task Create(CreateVM model);
		Task<IndexVM> GetAllChangesAsync(int page, int itemsPerPage, int count);

	}
}
