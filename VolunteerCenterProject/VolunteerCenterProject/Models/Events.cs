namespace VolunteerCenterProject.Models
{
	public class Events
	{
		public int EventId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime Deadline { get; set; }
		public int LocationId { get; set; }
		public Locations Location { get; set; }
		public decimal Budget { get; set; }

		public int CategoryId { get; set; }
		public Categories Category { get; set; }


		public int CreatedBy {  get; set; }
		public Users User { get; set; }

		// Event Status: Waiting, Assigned, InProgress, Completed, or Canceled
		public string Status { get; set; }

		public ICollection<VolunteerSignups> VolunteerSignups { get; set; }
		public ICollection<StatusHistory> StatusHistories { get; set; }
	}
}
