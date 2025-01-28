using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VolunteerCenterMVCProject.ViewModels.Locations
{
	public class CreateVM
	{

        [Required(ErrorMessage = "This filed is required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "This filed is required")]
		[Display(Name = "City")]
		public string City { get; set; }

		[Required(ErrorMessage = "This filed is required")]
		[Display(Name = "Country")]
		public string Country { get; set; }
	}
}
