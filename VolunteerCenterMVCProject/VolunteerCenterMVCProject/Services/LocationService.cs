﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VolunteerCenterMVCProject.Data;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.ViewModels.Locations;

namespace VolunteerCenterMVCProject.Services
{
	public class LocationService : ILocationService
	{
		private readonly ApplicationDbContext context;

		public LocationService(ApplicationDbContext context)
		{
			this.context = context;
		}



		public async Task CreateAsync(CreateVM model)
		{
			Location location = new Location()
			{
				Address = model.Address,
				City = model.City,
				Country = model.Country,
			};

			await context.Locations.AddAsync(location);
			await context.SaveChangesAsync();
		}
		public async Task UpdateAsync(EditVM model)
		{
			Location location = await context.Locations.FindAsync(model.Id);

			location.Address = model.Address;
			location.City = model.City;
			location.Country = model.Country;

			context.Locations.Update(location);
			await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(string id)
		{
			Location location = await context.Locations.FindAsync(id);

			var eventsToUpdate = context.Events.Where(e => e.LocationId == id);
			foreach (var ev in eventsToUpdate)
			{
				ev.LocationId = "1";
			}
			await context.SaveChangesAsync();


			context.Locations.Remove(location);
			await context.SaveChangesAsync();
		}

		public async Task<LocationVM> GetLocationAsync(string id)
		{
			Location location = await context.Locations.FindAsync(id);

			LocationVM item = new LocationVM()
			{
				Id = location.LocationId,
				Address = location.Address,
				City = location.City,
				Country = location.Country
			};
			return item;
		}

		public async Task<IndexVM> GetAllAsync(Expression<Func<LocationVM, bool>> filter = null)
		{

			IndexVM model = new IndexVM();

			IQueryable<LocationVM> query = context.Locations
				.Where(x => x.LocationId != "1")
				.Select(x => new LocationVM
				{
					Id = x.LocationId,
					Address = x.Address,
					City = x.City,
					Country = x.Country,
				});

			if (filter != null)
				query = query.Where(filter);


			model.Locations = await query.ToListAsync();

			return model;
		}

	}
}
