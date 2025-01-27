using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VolunteerCenterMVCProject.Data;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services.Interfaces;

namespace VolunteerCenterMVCProject.Services
{
	public class SignUpsService : ISignUpsService
	{
		private readonly ApplicationDbContext _context;

		public SignUpsService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<bool> SignUpForEventAsync(string eventId, string userId)
		{
			var existingSignup = await _context.Signups
				.FirstOrDefaultAsync(s => s.EventId == eventId && s.VolunteerId == userId);


			// Add a new signup record
			var signup = new VolunteerSignups
			{
				EventId = eventId,
				VolunteerId = userId,
				SignupDate = DateTime.Now,
				Status = "sms"
			};

			_context.Signups.Add(signup);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> CancelSignUpForEventAsync(string eventId, string userId)
		{
			// Find the signup record
			var signup = await _context.Signups
				.FirstOrDefaultAsync(s => s.EventId == eventId && s.VolunteerId == userId);


			_context.Signups.Remove(signup);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> IsUserSignedUpAsync(string eventId, string userId)
		{
			var isSignedUp = await _context.Signups
				.AnyAsync(s => s.EventId == eventId && s.VolunteerId == userId);

			return isSignedUp;
		}
	}
}
