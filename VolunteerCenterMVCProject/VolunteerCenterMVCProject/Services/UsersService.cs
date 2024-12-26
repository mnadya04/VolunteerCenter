using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using VolunteerCenterMVCProject.Common;
using VolunteerCenterMVCProject.Data;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Shared;
using VolunteerCenterMVCProject.ViewModels.Users;

namespace VolunteerCenterMVCProject.Services
{
	public class UsersService : IUsersService
	{

		private ApplicationDbContext context;
		private UserManager<User> userManager;
		private RoleManager<IdentityRole> roleManager;

		public UsersService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
		{
			this.context = context;
			this.userManager = userManager;
			this.roleManager = roleManager;
		}


		public int Count(Expression<Func<User, bool>> filter = null)
		{
			IQueryable<User> query = context.Users;

			if (filter != null)
				query = query.Where(filter);

			return query.Count();
		}
		public async Task CreateUserAsync(CreateUserVM model)
		{

			User user = new User()
			{
				Email = model.Email,
				NormalizedEmail = model.Email,
				EmailConfirmed = true,
				SecurityStamp = string.Empty,
				UserName = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				NormalizedUserName = model.Email
			};


			await this.userManager.CreateAsync(user, model.Password);

			User item = await userManager.FindByNameAsync(user.Email);

			await userManager.AddToRoleAsync(user,Constants.VolunteerRole);

			/*bool roleExist = await roleManager.RoleExistsAsync(Constants.VolunteerRole);

			if (roleExist)
			{
				var result = await userManager.AddToRoleAsync(item, Constants.VolunteerRole);

				if (!result.Succeeded)
				{
					throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
				}
			}*/
		}
		public async Task DeleteUserByIdAsync(string id)
		{
			User item = await this.userManager.FindByIdAsync(id);

			await userManager.DeleteAsync(item);
		}
		public async Task UpdateUserAsync(EditUserVM model)
		{
			User item = await context.Users.FindAsync(model.Id);

			item.FirstName = model.FirstName;
			item.LastName = model.LastName;

			await context.SaveChangesAsync();

		}
		
		public async Task<UserVM> GetUserByIdAsync(string id)
		{
			User item = await context.Users.FindAsync(id);

			UserVM model = null;

			if(item != null)
			{
				model = new UserVM()
				{
					FirstName = item.FirstName,
					LastName = item.LastName,
					Email = item.Email,
					Id = item.Id
				};
			}

			return model;

		}
		public async Task<EditUserVM> GetUserEditByIdAsync(string id)
		{
			User item = await context.Users.FindAsync(id);
			EditUserVM model = null;

			if (item != null)
			{
				model = new EditUserVM()
				{
					Id = item.Id,
					FirstName = item.FirstName,
					LastName = item.LastName
				};
			}

			return model;
		}

		
		public async Task<UsersVM> GetUsersAsync(int page = 1, int itemsPerPage = 1, int count = 10)
		{

			UsersVM model = new UsersVM()
			;

			model.Users = await this.context.Users
				.Skip((page - 1) * itemsPerPage)
				.Take(itemsPerPage)
				.Select(x => new UserVM()
				{
					Id = x.Id,
					FirstName = x.FirstName,
					LastName = x.LastName,
					Email = x.Email
				})
				.ToListAsync();


			return model;
		}


	}
}
