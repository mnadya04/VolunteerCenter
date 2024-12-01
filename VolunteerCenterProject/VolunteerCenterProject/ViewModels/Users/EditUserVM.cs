using System.ComponentModel.DataAnnotations;

namespace VolunteerCenterProject.ViewModels.Users
{
	public class EditUserVM
	{
		[Required(ErrorMessage ="This filed is required")]
		[Display(Name = "First Name")]

		public string FirstName {  get; set; }
		
		[Required(ErrorMessage ="This filed is required")]
		[Display(Name = "Last Name")]

		public string LastName {  get; set; }
	}
}
