using System.ComponentModel.DataAnnotations;

namespace VolunteerCenterMVCProject.ViewModels.Events
{
	public class EditEventVM
	{
		public string Id { get; set; }
		
		
		[Required]
		[Display(Name="Name")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Description")]
		public string Description { get; set; }


	}
}
