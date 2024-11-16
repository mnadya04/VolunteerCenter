namespace VolunteerCenterProject.Models
{
	public class Users
	{
		public int UserId { get; set; }
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string Role { get; set; }

		public ICollection<VolunteerSignups> VolunteerSignups { get; set; }

		public ICollection<Events> Events { get; set; }

		public ICollection<StatusHistory> StatusHistories { get; set; }
	}
}
