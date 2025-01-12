using VolunteerCenterMVCProject.ViewModels.Shared;

namespace VolunteerCenterMVCProject.ViewModels.StatusHistory
{
	public class IndexVM
	{		
		public ICollection<StatusChangeVM> Changes { get; set; }
		public PagerVM Pager { get; set; }

	}
}
