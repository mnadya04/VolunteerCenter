using System.Linq.Expressions;
using VolunteerCenterMVCProject.ViewModels.Categories;

namespace VolunteerCenterMVCProject.Services.Interfaces
{
    public interface ICategoriesService
    {
        int Count(Expression<Func<IndexVM, bool>> filter = null);

        Task CreateAsync(CreateCategoryVM model);
        Task DeleteByIdAsync(string id);
        Task<CategoryVM> GetCategoryAsync(string id);
        Task UpdateAsync(CategoryVM model);
        Task<IndexVM> GetAllAsync(Expression<Func<CategoryVM, bool>> filter, int page, int itemsPerPage, int count);
    }
}
