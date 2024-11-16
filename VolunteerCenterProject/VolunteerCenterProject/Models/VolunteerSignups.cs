using Microsoft.Extensions.Logging;

namespace VolunteerCenterProject.Models
{
	public class VolunteerSignups
	{
		public int SignupId { get; set; }

		public int VolunteerId { get; set; }
		public Users User { get; set; }

		public int EventId { get; set; }
		public Events Event { get; set; }

		// Current Status for Volunteer in this Event
		public string Status { get; set; } // E.g., SignedUp, InProgress, Completed, Canceled

		public DateTime SignupDate { get; set; }
	}
}
