using Microsoft.EntityFrameworkCore;
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

            //delete connected event

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

            IQueryable<CategoryVM> query = context.Categories.Select(x => new CategoryVM()
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

            var category = await context.Categories
                .Include(c => c.Events)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            IndexEventsViewModel allEvents = await eventsService.GetEventsAsync(new IndexEventsViewModel());

            List<string> assignedEventIds = category.Events.Select(e => e.EventId).ToList();

            List<IndexEventViewModel> availableEvents = allEvents.Events
                .Where(e => !assignedEventIds.Contains(e.Id))
                .Select(e => new IndexEventViewModel
                {
                    Id = e.Id,
                    Name = e.Name
                })
                .ToList();

            return new IndexCategoryEventsViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.Name,
                AssignedEvents = category.Events.Select(e => new IndexEventViewModel
                {
                    Id = e.EventId,
                    Name = e.Name
                }).ToList(),
                AvailableEvents = availableEvents
            };
        }


        public async Task AddEventToCategoryAsync(string categoryId, string eventId)
        {
            Category? category = await context.Categories.Include(c => c.Events).FirstOrDefaultAsync(c => c.CategoryId == categoryId);
            EditEventViewModel eventToAdd = await eventsService.GetEventToEditAsync(eventId);

            if (category == null || eventToAdd == null)
                throw new KeyNotFoundException("Invalid category or event.");

            if (!category.Events.Any(e => e.EventId == eventToAdd.Id))
            {
                category.Events.Add(new Event
                {
                    EventId = eventToAdd.Id,
                    Name = eventToAdd.Name,
                    Deadline = eventToAdd.Deadline,
                    Budget = eventToAdd.Budget
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveEventFromCategoryAsync(string categoryId, string eventId)
        {
            Category? category = await context.Categories.Include(c => c.Events).FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            Event? eventToRemove = category.Events.FirstOrDefault(e => e.EventId == eventId);

            if (eventToRemove != null)
            {
                category.Events.Remove(eventToRemove);
                await context.SaveChangesAsync();
            }
        }

    }
}

