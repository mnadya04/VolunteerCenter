using System.Globalization;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.ViewModels.Shared;

namespace VolunteerCenterMVCProject.ViewModels.Categories
{
    public class IndexVM
    {
        public string Id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<CategoryVM> Categories { get; set; }

        public PagerVM Pager {  get; set; }
    }
}
