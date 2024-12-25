using Microsoft.AspNetCore.Mvc.Rendering;
using VolunteerCenterMVCProject.ViewModels.Users;

namespace VolunteerCenterMVCProject.Services.Interfaces
{
	public interface IUsersService
	{

		Task CreateUserAsync(CreateUserVM model);

		Task<UserVM> GetUserByIdAsync(string id);
		Task<UsersVM> GetUsersAsync(int page = 1,int itemsPerPage = 2, int count = 10);
		Task DeleteUserByIdAsync(string id);
		Task UpdateUserAsync(EditUserVM model);
		Task<SelectList> GetAllUsersAsync();
	}
}
