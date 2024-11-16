using Microsoft.Extensions.Logging;

namespace VolunteerCenterProject.Models
{
	public class StatusHistory
	{
		public int StatusHistoryId { get; set; }

		// Foreign Key for Event
		public int EventId { get; set; }
		public Events Event { get; set; }

		public int ChangedBy { get; set; }
		public Users User { get; set; }

		public string NewStatus { get; set; } // E.g., Waiting, Assigned, InProgress, Completed, Canceled
		public DateTime ChangeDate { get; set; }
		//public string Notes { get; set; } // Optional notes regarding the status change
	//	public string Attachment { get; set; } 
	}
}
