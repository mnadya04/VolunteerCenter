using Microsoft.Extensions.Logging;

namespace VolunteerCenterProject.Models
{
	public class Locations
	{
		public int LocationId { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Country { get; set; }

		public ICollection<Events> Events { get; set; }
	}
}
