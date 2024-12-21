using Microsoft.Extensions.Logging;

namespace VolunteerCenterProject.Models
{
	public class StatusHistory
	{
		public StatusHistory()
		{
			this.EventId = Guid.NewGuid().ToString();
		}		
		
		public string EventId { get; set; }

		public int StatusHistoryId { get; set; }

		public virtual Event Event { get; set; }

		public string ChangedBy { get; set; }
		public virtual User User { get; set; }

		public string NewStatus { get; set; } // E.g., Waiting, Assigned, InProgress, Completed, Canceled
		public DateTime ChangeDate { get; set; }
		//public string Notes { get; set; } // Optional notes regarding the status change
	//	public string Attachment { get; set; } 
	}
}
