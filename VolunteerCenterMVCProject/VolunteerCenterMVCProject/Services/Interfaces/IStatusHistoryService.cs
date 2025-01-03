using System.Linq.Expressions;
using VolunteerCenterMVCProject.ViewModels.StatusHistory;

namespace VolunteerCenterMVCProject.Services.Interfaces
{
	public interface IStatusHistoryService
	{
		//int Count
		Task Create(StatusChangeVM model);
		Task<IndexVM> GetAllChanges(Expression<Func<StatusChangeVM, bool>> filter, int page, int itemsPerPage, int count);

	}
}
