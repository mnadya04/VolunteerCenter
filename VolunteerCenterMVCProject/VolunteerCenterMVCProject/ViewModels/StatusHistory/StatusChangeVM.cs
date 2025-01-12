using VolunteerCenterMVCProject.Models;

namespace VolunteerCenterMVCProject.ViewModels.StatusHistory
{
	public class StatusChangeVM
	{
		public string Username { get; set; }
		public string Event { get; set; }
		public string NewStatus { get; set; } // E.g., Waiting, Assigned, InProgress, Completed, Canceled
		public DateTime ChangeDate { get; set; }


	}
}
