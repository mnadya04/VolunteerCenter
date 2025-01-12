
namespace VolunteerCenterMVCProject.ViewModels.Locations
{
	public class IndexVM
	{

		public string Id {  get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public ICollection<LocationVM> Locations { get; set; }

	}
}
