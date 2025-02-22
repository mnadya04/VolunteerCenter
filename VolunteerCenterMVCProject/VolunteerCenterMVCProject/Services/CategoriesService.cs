﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using VolunteerCenterMVCProject.Data;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Categories;
using VolunteerCenterMVCProject.ViewModels.Events;

namespace VolunteerCenterMVCProject.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext context;
        private readonly IEventsService eventsService;

        public CategoriesService(ApplicationDbContext context, IEventsService eventsService)
        {
            this.context = context;
            this.eventsService = eventsService;
        }

        public int Count(Expression<Func<CategoryVM, bool>> filter = null)
        {
            IQueryable<CategoryVM> query = context.Categories.Select(x => new CategoryVM()
            {
                Description = x.Description,
                Name = x.Name,
                Id = x.CategoryId
            });

            if (filter != null)
                query = query.Where(filter);

            return query.Count();
        }

        public async Task CreateAsync(CreateVM model)
        {
            Category category = new Category()
            {
                Description = model.Description,
                Name = model.Name
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(EditVM model)
        {
            Category category = await context.Categories.FindAsync(model.Id);
            
            category.Name = model.Name;
            category.Description = model.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(string id)
        {
            Category category = await context.Categories.FindAsync(id);

			var eventsToUpdate = context.Events.Where(e => e.CategoryId == id);
			foreach (var ev in eventsToUpdate)
			{
				ev.CategoryId = "1";
			}
			await context.SaveChangesAsync();

			context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }
        public async Task<CategoryVM> GetCategoryAsync(string id)
        {
            Category category = await context.Categories.FindAsync(id);

            CategoryVM model = new CategoryVM()
            {
                Description = category.Description,
                Name = category.Name,
                Id = category.CategoryId
            };

            return model;
        }

        public async Task<IndexVM> GetAllAsync(Expression<Func<CategoryVM, bool>> filter, int page, int itemsPerPage, int count)
        {

            IndexVM model = new IndexVM();

            IQueryable<CategoryVM> query = context.Categories
                .Where(x => x.CategoryId != "1")
				.Select(x => new CategoryVM()
            {
                Description = x.Description,
                Name = x.Name,
                Id = x.CategoryId
            });

            if (filter != null)
                query = query.Where(filter);

            model.Categories = await query
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToListAsync();

            return model;
        }

        public async Task<IndexCategoryEventsViewModel> GetCategoryWithEventsAsync(string categoryId)
        {
            var category = await this.context.Categories
                .Where(c => c.CategoryId == categoryId)
                .Include(c => c.Events) // Include events
                .FirstOrDefaultAsync();

            if (category == null) return null;

            return new IndexCategoryEventsViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.Name,
                Events = category.Events.Select(e => new IndexEventViewModel
                {
                    Id = e.EventId,
                    Name = e.Name,
                }).ToList()
            };
        }

	
	}
}

