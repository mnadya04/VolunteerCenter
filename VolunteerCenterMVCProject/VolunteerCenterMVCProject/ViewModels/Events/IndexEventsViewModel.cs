using System;
using VolunteerCenterMVCProject.ViewModels.Shared;

namespace VolunteerCenterMVCProject.ViewModels.Events
{
	public class IndexEventsViewModel
	{
        public ICollection<IndexEventViewModel> Events { get; set; }
        public PagerVM Pager { get; internal set; }
    }
}

