namespace VolunteerCenterProject.Models
{
	public class Event
	{
		public Event()
		{
			this.EventId = Guid.NewGuid().ToString();
		}
		public string EventId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime Deadline { get; set; }
		public string LocationId { get; set; }
		public virtual Location Location { get; set; }
		public double Budget { get; set; }

		public string CategoryId { get; set; }
		public virtual Category Category { get; set; }


		public string CreatedBy {  get; set; }
		public virtual User User { get; set; }

		// Event Status: Waiting, Assigned, InProgress, Completed, or Canceled
		public string Status { get; set; }

		public virtual ICollection<VolunteerSignups> VolunteerSignups { get; set; }
		public virtual ICollection<StatusHistory> StatusHistories { get; set; }
	}
}
