using VolunteerCenterMVCProject.ViewModels.Shared;

namespace VolunteerCenterMVCProject.ViewModels.Users
{
	public class UsersVM
	{
		public ICollection<UserVM> Users { get; set; }
		public PagerVM Pager { get; set; }
	}
}
