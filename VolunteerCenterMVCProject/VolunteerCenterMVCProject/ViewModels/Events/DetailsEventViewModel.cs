using System;
using VolunteerCenterMVCProject.Models;

namespace VolunteerCenterMVCProject.ViewModels.Events
{
	public class DetailsEventViewModel
	{
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string Location { get; set; }
        public double Budget { get; set; }
        public string Category { get; set; }
        public string User { get; set; }

        // Event Status: Waiting, Assigned, InProgress, Completed, or Canceled
        public string Status { get; set; }

        public virtual ICollection<VolunteerSignups> VolunteerSignups { get; set; }
    }
}

