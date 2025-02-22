﻿using Microsoft.Extensions.Logging;

namespace VolunteerCenterMVCProject.Models
{
	public class VolunteerSignups
	{
		public VolunteerSignups()
		{
			this.SignupId = Guid.NewGuid().ToString();
		}
		public string SignupId { get; set; }

		public string VolunteerId { get; set; }
		public virtual User User { get; set; }

		public string EventId { get; set; }
		public virtual Event Event { get; set; }
		public DateTime SignupDate { get; set; }
	}
}
