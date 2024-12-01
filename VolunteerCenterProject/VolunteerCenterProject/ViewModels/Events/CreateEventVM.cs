using System.ComponentModel.DataAnnotations;

namespace VolunteerCenterProject.ViewModels.Events
{
	public class CreateEventVM
	{
	[Required]
		public string Name { get; set; }
		
		[Required]
		public string Description { get; set; }
		
		[Required]
		public DateTime Deadline { get; set; }
		
		[Required]
		public string LocationId { get; set; }

		//[Required]
		public double Budget { get; set; }

		[Required]
		public string CategoryId { get; set; }

		[Required]
		public string CreatedBy { get; set; }
	
	}
}
