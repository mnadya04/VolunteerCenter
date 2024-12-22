using Microsoft.AspNetCore.Identity;

namespace VolunteerCenterMVCProject.Models
{
	public class User : IdentityUser<string>
	{
		public User()
		{
			this.Id = Guid.NewGuid().ToString();
		}
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public virtual ICollection<VolunteerSignups> VolunteerSignups { get; set; }

		public virtual ICollection<Event> Events { get; set; }

		public virtual ICollection<StatusHistory> StatusHistories { get; set; }
	}
}
