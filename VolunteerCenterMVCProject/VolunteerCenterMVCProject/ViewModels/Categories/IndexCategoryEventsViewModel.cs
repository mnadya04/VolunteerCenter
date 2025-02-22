﻿using VolunteerCenterMVCProject.ViewModels.Events;

namespace VolunteerCenterMVCProject.ViewModels.Categories
{
    public class IndexCategoryEventsViewModel
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<IndexEventViewModel> Events { get; set; }
    }
}