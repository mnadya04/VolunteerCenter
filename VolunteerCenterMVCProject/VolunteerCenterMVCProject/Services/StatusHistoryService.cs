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
		private readonly ApplicationDbContext context;

		public StatusHistoryService(ApplicationDbContext context)
		{
			this.context = context;
		}

		public int Count()
		{
			return context.statusHistories.Count();
		}
		public async Task Create(CreateVM model)
		{
			StatusHistory change = new StatusHistory()
			{
				ChangedBy = model.UserId,
				EventId = model.EventId,
				NewStatus = model.NewStatus,
				ChangeDate = DateTime.Now,
			};

			await context.statusHistories.AddAsync(change);
			await context.SaveChangesAsync();
		}



		public async Task<IndexVM> GetAllChangesAsync
			(int page, int itemsPerPage, int count)
		{
			IndexVM model = new IndexVM();

			IQueryable<StatusChangeVM> query =
				context.statusHistories
				.Include(x => x.User)
				.Include(x => x.Event)
				.OrderByDescending(x => x.ChangeDate)
				.Select(x => new StatusChangeVM()
				{
					Event = x.Event.Name,
					Username = x.User.UserName,
					NewStatus = x.NewStatus,
					ChangeDate = x.ChangeDate,
				});

			model.Changes = await query
					.OrderBy(x => x.Event)
					.ThenByDescending(x => x.ChangeDate)
					.Skip((page - 1) * itemsPerPage)
					.Take(itemsPerPage)
					.ToListAsync();

			return model;
		}
	}
}
