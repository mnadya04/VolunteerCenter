using Microsoft.Extensions.Logging;

namespace VolunteerCenterProject.Models
{
	public class Location
	{

		public Location()
		{
			this.LocationId = Guid.NewGuid().ToString();
		}
		public string LocationId { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Country { get; set; }

		public virtual ICollection<Event> Events { get; set; }
	}
}
