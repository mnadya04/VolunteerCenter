using Microsoft.Extensions.Logging;

namespace VolunteerCenterProject.Models
{
	public class Categories
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		// Navigation Property
		public ICollection<Events> Events { get; set; }
	}
}
