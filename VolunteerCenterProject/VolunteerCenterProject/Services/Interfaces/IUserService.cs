﻿using Microsoft.AspNetCore.Mvc.Rendering;
using VolunteerCenterProject.ViewModels.Users;

namespace VolunteerCenterProject.Services.Interfaces
{
	public interface IUserService
	{

		Task CreateAsync(CreateUserVM model);

		Task<UserVM> GetUserByIdAsync(string id);
		Task<UsersVM> GetUsersAsync(int page = 1, int count = 10);
		Task DeleteUserByIdAsync(string id);
		Task UpdateUserAsync(EditUserVM model);
		Task<SelectList> GetAllUsersAsync();
	}
}
