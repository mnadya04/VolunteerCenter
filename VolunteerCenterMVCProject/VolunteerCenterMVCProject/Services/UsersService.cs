﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VolunteerCenterMVCProject.Common;
using VolunteerCenterMVCProject.Data;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services.Interfaces;
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

        public async Task CreateAsync(CreateUserVM model)
        {

            User user = new User()
            {
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName ,
                NormalizedUserName = model.Email.ToUpper()
            };

           
            await this.userManager.CreateAsync(user, model.Password);

            //check if there is problem with adding new user
         /*   if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    // Log the error or handle it accordingly
                    Console.WriteLine(error.Description);
                }
                return;
            }*/
            User item = await userManager.FindByNameAsync(user.Email);

            bool roleExist = await roleManager.RoleExistsAsync(Constraints.VolunteerRole);

            if (roleExist)
            {
                var result = await userManager.AddToRoleAsync(item, Constraints.VolunteerRole);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
    }

        public async Task DeleteUserByIdAsync(string id)
        {
            User item = await this.userManager.FindByIdAsync(id);

            await userManager.DeleteAsync(item);
        }

        public async Task<SelectList> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<UserVM> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<UsersVM> GetUsersAsync(int page = 1, int itemsPerPage = 2, int count = 10)
        {

            UsersVM model = new UsersVM();

            model.Users = await this.context.Users
                .Select(x => new UserVM()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email
                })
                //.Skip((page - 1) * itemsPerPage)
                //.Take(itemsPerPage)
                .ToListAsync();


            return model;
        }

        public Task UpdateUserAsync(EditUserVM model)
        {
            throw new NotImplementedException();
        }
    }
}
