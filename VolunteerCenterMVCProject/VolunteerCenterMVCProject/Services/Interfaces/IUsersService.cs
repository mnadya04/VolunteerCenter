using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.ViewModels.Users;

namespace VolunteerCenterMVCProject.Services.Interfaces
{
	public interface IUsersService
	{

		Task CreateUserAsync(CreateUserVM model);

		Task<EditUserVM> GetUserEditByIdAsync(string id);

		Task DeleteUserByIdAsync(string id);
		Task UpdateUserAsync(EditUserVM model);
		int Count(Expression<Func<User, bool>> filter = null);
		Task<UserVM> GetUserByIdAsync(string id);
		Task<UsersVM> GetUsersAsync(int page = 1, int itemsPerPage = 1, int count = 10);
	}
}
