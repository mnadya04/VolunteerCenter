using System;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.ViewModels.Users;

namespace VolunteerCenterMVCProject.ViewModels.Events
{
	public class IndexEventViewModel
	{
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime Deadline { get; set; }
        public double Budget { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
    }
}

