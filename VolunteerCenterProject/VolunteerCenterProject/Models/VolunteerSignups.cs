using Microsoft.Extensions.Logging;

namespace VolunteerCenterProject.Models
{
	public class VolunteerSignups
	{
		public VolunteerSignups()
		{
			this.SignupId = Guid.NewGuid().ToString();
		}
		public string SignupId { get; set; }

		public string VolunteerId { get; set; }
		public virtual User User { get; set; }

		public string EventId { get; set; }
		public virtual Event Event { get; set; }

		// Current Status for Volunteer in this Event
		public string Status { get; set; } // E.g., SignedUp, InProgress, Completed, Canceled

		public DateTime SignupDate { get; set; }
	}
}
