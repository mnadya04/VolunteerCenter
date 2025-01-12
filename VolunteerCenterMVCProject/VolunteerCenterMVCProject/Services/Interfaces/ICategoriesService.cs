using System.Linq.Expressions;
using VolunteerCenterMVCProject.ViewModels.Categories;

namespace VolunteerCenterMVCProject.Services.Interfaces
{
    public interface ICategoriesService
    {
        int Count(Expression<Func<CategoryVM, bool>> filter = null);

        Task CreateAsync(CreateVM model);
        Task DeleteByIdAsync(string id);
        Task<CategoryVM> GetCategoryAsync(string id);
        Task UpdateAsync(EditVM model);
        Task<IndexVM> GetAllAsync(Expression<Func<CategoryVM, bool>> filter, int page, int itemsPerPage, int count);
    }
}
