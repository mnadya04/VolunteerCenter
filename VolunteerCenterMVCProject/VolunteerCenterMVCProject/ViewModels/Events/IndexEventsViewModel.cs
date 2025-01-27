using System;
using VolunteerCenterMVCProject.ViewModels.Shared;

namespace VolunteerCenterMVCProject.ViewModels.Events
{
	public class IndexEventsViewModel
	{
		public string Name { get; set; }
		public string Location { get; set; }
		public string Category { get; set; }
		public ICollection<IndexEventViewModel> Events { get; set; }
        public PagerVM Pager { get; internal set; }
    }
}

