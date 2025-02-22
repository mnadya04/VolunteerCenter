﻿using Microsoft.Extensions.Logging;

namespace VolunteerCenterMVCProject.Models
{
	public class StatusHistory
	{
		public StatusHistory()
		{
			this.StatusHistoryId = Guid.NewGuid().ToString();
		}


		public string EventId { get; set; }

		public string StatusHistoryId { get; set; }

		public virtual Event Event { get; set; }

		public string ChangedBy { get; set; }
		public virtual User User { get; set; }

		public string NewStatus { get; set; } // E.g., Waiting, Assigned, InProgress, Completed, Canceled
		public DateTime ChangeDate { get; set; }
	}
}
