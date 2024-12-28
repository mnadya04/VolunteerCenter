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


		public int Count(Expression<Func<UserVM, bool>> filter = null)
		{
			IQueryable<UserVM> query = this.context.Users.
							Select(x => new UserVM()
							{
								Id = x.Id,
								FirstName = x.FirstName,
								LastName = x.LastName,
								Email = x.Email
							});

			if (filter != null)
				query = query.Where(filter);

			return query.Count();
		}
		public async Task CreateAsync(CreateUserVM model)
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

			await userManager.AddToRoleAsync(user, Constants.VolunteerRole);

			bool roleExist = await roleManager.RoleExistsAsync(Constants.VolunteerRole);

			if (roleExist)
			{
				var result = await userManager.AddToRoleAsync(item, Constants.VolunteerRole);

				if (!result.Succeeded)
				{
					throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
				}
			}
		}
		public async Task DeleteAsync(string id)
		{
			User item = await this.userManager.FindByIdAsync(id);

			await userManager.DeleteAsync(item);
		}
		public async Task UpdateAsync(EditUserVM model)
		{
			User item = await context.Users.FindAsync(model.Id);

			item.FirstName = model.FirstName;
			item.LastName = model.LastName;

			await context.SaveChangesAsync();

		}

		public async Task<UserVM> GetUserAsync(string id)
		{
			User item = await context.Users.FindAsync(id);

			UserVM model = null;

			if (item != null)
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
		public async Task<EditUserVM> EditAsync(string id)
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


		public async Task<IndexVM> GetAllAsync(Expression<Func<UserVM, bool>> filter, int page, int itemsPerPage, int count)
		{

			IndexVM model = new IndexVM();

			IQueryable<UserVM> query = this.context.Users.
				Select(x => new UserVM()
			{
				Id = x.Id,
				FirstName = x.FirstName,
				LastName = x.LastName,
				Email = x.Email
			});

			if (filter != null)
				query = query.Where(filter);


			model.Users = await query
					.Skip((page - 1) * itemsPerPage)
					.Take(itemsPerPage)
					.ToListAsync();


			return model;
		}


	}
}
