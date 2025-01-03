using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using VolunteerCenterMVCProject.Data;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.StatusHistory;

namespace VolunteerCenterMVCProject.Services
{
	public class StatusHistoryService : IStatusHistoryService
	{
		private ApplicationDbContext context;

		public StatusHistoryService(ApplicationDbContext context)
		{
			this.context = context;
		}
		public async Task Create(StatusChangeVM model)
		{
			StatusHistory change = new StatusHistory()
			{
				ChangedBy = model.UserId,
				EventId = model.EvntId,
				NewStatus = model.NewStatus,
				ChangeDate = DateTime.Now,
			};

			await context.statusHistories.AddAsync(change);
			await context.SaveChangesAsync();
		}

		public async Task<IndexVM> GetAllChanges(Expression<Func<StatusChangeVM, bool>> filter, int page, int itemsPerPage, int count)
		{
			IndexVM model = new IndexVM();

			IQueryable<StatusChangeVM> query = 
				context.statusHistories.Select(x => new StatusChangeVM()
			{
				UserId = x.ChangedBy,
				EvntId = x.EventId,
				NewStatus = x.NewStatus,
				ChangeDate = DateTime.Now,
			});

			if (filter != null)
				query = query.Where(filter);

			model.Changes = await query
					.Skip((page - 1) * itemsPerPage)
					.Take(itemsPerPage)
					.OrderBy(x => x.ChangeDate)
					.ToListAsync();

			return model;
		}
	}
}
