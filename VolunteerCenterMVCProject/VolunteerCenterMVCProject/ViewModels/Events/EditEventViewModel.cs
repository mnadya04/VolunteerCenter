using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using VolunteerCenterMVCProject.Models;

namespace VolunteerCenterMVCProject.ViewModels.Events
{
	public class EditEventViewModel
	{
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string LocationId { get; set; }
        //public virtual Location Location { get; set; }
        public double Budget { get; set; }

        public string CategoryId { get; set; }


        public string CreatedBy { get; set; }

        public string LocationCity { get; set; }

        public string CategoryName { get; set; }


        public List<SelectListItem> LocationOptions { get; set; }
        public List<SelectListItem> CategoryOptions { get; set; }
        public List<SelectListItem> StatusOptions { get; set; }

        // Event Status: Waiting, Assigned, InProgress, Completed, or Canceled
        public string Status { get; set; }

        public ICollection<VolunteerSignups> VolunteerSignups { get; set; }
    }
}

