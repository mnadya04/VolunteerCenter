using Microsoft.Extensions.Logging;

namespace VolunteerCenterMVCProject.Models
{
	public class Category
	{
		public Category()
		{
			this.CategoryId = Guid.NewGuid().ToString();
		}
		public string CategoryId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		// Navigation Property
		public virtual ICollection<Event> Events { get; set; }
	}
}
