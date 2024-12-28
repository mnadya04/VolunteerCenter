using System.ComponentModel.DataAnnotations;

namespace VolunteerCenterMVCProject.ViewModels.Users
{
	public class EditUserVM
	{
		public string Id { get; set; }


		[Display(Name = "First Name")]
		public string FirstName {  get; set; }

		
		[Display(Name = "Last Name")]
		public string LastName {  get; set; }
	}
}
