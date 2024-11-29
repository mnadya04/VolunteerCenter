using Microsoft.Extensions.Logging;

namespace VolunteerCenterProject.Models
{
	public class Categories
	{
		public Categories()
		{
			this.CategoryId = Guid.NewGuid().ToString();
		}
		public string CategoryId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		// Navigation Property
		public virtual ICollection<Events> Events { get; set; }
	}
}
