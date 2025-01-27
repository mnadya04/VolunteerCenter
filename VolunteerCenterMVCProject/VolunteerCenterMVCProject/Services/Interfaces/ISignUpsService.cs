namespace VolunteerCenterMVCProject.Services.Interfaces
{
	public interface ISignUpsService
	{
		Task<bool> SignUpForEventAsync(string eventId, string userId);
		Task<bool> CancelSignUpForEventAsync(string eventId, string userId);
		Task<bool> IsUserSignedUpAsync(string eventId, string userId);
	}
}
