using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.ViewModels.Users;

namespace VolunteerCenterMVCProject.Services.Interfaces
{
	public interface IUsersService
	{
		int Count(Expression<Func<UserVM, bool>> filter = null);

		Task CreateAsync(CreateUserVM model);
		Task DeleteAsync(string id);
		Task UpdateAsync(EditUserVM model);
		Task<UserVM> GetUserAsync(string id);
		Task<EditUserVM> EditAsync(string id);
		Task<IndexVM> GetAllAsync(Expression<Func<UserVM, bool>> filter,int page, int itemsPerPage, int count);
	}
}
