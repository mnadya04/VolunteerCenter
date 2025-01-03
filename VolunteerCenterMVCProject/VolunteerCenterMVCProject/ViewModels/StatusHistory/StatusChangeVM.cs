using VolunteerCenterMVCProject.Models;

namespace VolunteerCenterMVCProject.ViewModels.StatusHistory
{
	public class StatusChangeVM
	{
		public string UserId {  get; set; }//ChangedBy
		public string EvntId { get; set; }
		public string NewStatus { get; set; } // E.g., Waiting, Assigned, InProgress, Completed, Canceled
		public DateTime ChangeDate { get; set; }


	}
}
