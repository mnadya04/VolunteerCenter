using VolunteerCenterMVCProject.ViewModels.Shared;

namespace VolunteerCenterMVCProject.ViewModels.Users
{
	public class IndexVM
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public ICollection<UserVM> Users { get; set; }
		public PagerVM Pager { get; set; }
	}
}
