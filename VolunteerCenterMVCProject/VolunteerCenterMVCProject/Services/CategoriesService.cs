using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VolunteerCenterMVCProject.Data;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Categories;

namespace VolunteerCenterMVCProject.Services
{
    public class CategoriesService : ICategoriesService
    {
        private ApplicationDbContext context;


        public CategoriesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public int Count(Expression<Func<IndexVM, bool>> filter = null)
        {
            IQueryable<IndexVM> query = context.Categories.Select(x => new IndexVM()
            {
                Description = x.Description,
                Name = x.Name,
                Id = x.CategoryId
            });

            if (filter != null)
                query = query.Where(filter);

            return query.Count();
        }

        public async Task CreateAsync(CreateCategoryVM model)
        {
            Category category = new Category()
            {
                Description = model.Description,
                Name = model.Name
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(CategoryVM model)
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

       
    }
}

