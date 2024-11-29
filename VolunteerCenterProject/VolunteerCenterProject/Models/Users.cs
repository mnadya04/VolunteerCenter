using Microsoft.AspNetCore.Identity;

namespace VolunteerCenterProject.Models
{
	public class Users : IdentityUser<string>
	{
		public Users()
		{
			this.Id = Guid.NewGuid().ToString();
		}
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public virtual ICollection<VolunteerSignups> VolunteerSignups { get; set; }

		public virtual ICollection<Events> Events { get; set; }

		public virtual ICollection<StatusHistory> StatusHistories { get; set; }
	}
}
