using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VolunteerCenterMVCProject.ViewModels.Users
{
	public class CreateVM
	{
		[Required(ErrorMessage = "This filed is required")]
		[Display(Name = "Email")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "This filed is required")]
        [DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required(ErrorMessage = "This filed is required")]
		[Display(Name = "Confirm Password")]
		[Compare("Password",ErrorMessage ="Password and Confirm password do not match")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "This filed is required")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "This filed is required")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }
	}


}
