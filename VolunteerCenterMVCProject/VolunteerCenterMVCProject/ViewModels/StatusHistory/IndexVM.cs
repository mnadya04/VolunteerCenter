using VolunteerCenterMVCProject.ViewModels.Shared;

namespace VolunteerCenterMVCProject.ViewModels.StatusHistory
{
	public class IndexVM
	{
		public int EventId {  get; set; }//event id
		
		public string Title { get; set; }
		public int UserId {  get; set; }

		public string Username {  get; set; }
		public ICollection<StatusChangeVM> Changes { get; set; }
		public PagerVM Pager { get; set; }

	}
}
