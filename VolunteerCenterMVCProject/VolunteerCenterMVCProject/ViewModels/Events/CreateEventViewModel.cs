using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VolunteerCenterMVCProject.ViewModels.Events
{
    public class CreateEventViewModel
    {

        public string CreatedBy {  get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The Name field must be less than 100 characters.")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }

        public string LocationId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive value.")]
        public double Budget { get; set; }

        public string CategoryId { get; set; }

        [Required]
        public string Status { get; set; }

        public List<SelectListItem> LocationOptions { get; set; }
        public List<SelectListItem> CategoryOptions { get; set; }
        public List<SelectListItem> StatusOptions { get; set; }
    }
}
