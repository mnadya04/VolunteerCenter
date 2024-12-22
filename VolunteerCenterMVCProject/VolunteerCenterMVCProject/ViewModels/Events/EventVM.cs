namespace VolunteerCenterMVCProject.ViewModels.Events
{
	public class EventVM
	{

		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime Deadline { get; set; }
		public string LocationId { get; set; }
		public double Budget { get; set; }
		public string CategoryId { get; set; }
		public string CreatedBy { get; set; }

	}
}
