using System.ComponentModel.DataAnnotations;

namespace VolunteerCenterMVCProject.ViewModels.Categories
{
    public class CreateVM
    {


        [Required(ErrorMessage = "This filed is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }


    }
}
