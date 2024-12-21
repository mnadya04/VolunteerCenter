﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection.Emit;
using VolunteerCenterProject.Models;

namespace VolunteerCenterProject.Data
{
	public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public  DbSet<Location> Locations { get; set; }
		public  DbSet<Category> Categories { get; set; }
		public  DbSet<VolunteerSignups> Signups { get; set; }
		//override public  DbSet<User> Users { get; set; } //-- removed because IdentityDbContext<Users.. will set up 
		public  DbSet<StatusHistory> statusHistories { get; set; }
		public  DbSet<Event> Events { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); 

			#region Categories
			modelBuilder.Entity<Category>()
				.HasKey(x => x.CategoryId);
			#endregion

			#region Locations
			modelBuilder.Entity<Location>()
				.HasKey(c => c.LocationId);
			#endregion

			#region VolunteerSingups

			modelBuilder.Entity<VolunteerSignups>()
				.HasKey(c => c.SignupId);

			modelBuilder.Entity<VolunteerSignups>()
				.HasOne(x => x.User)
				.WithMany(u => u.VolunteerSignups)
				.HasForeignKey(x => x.VolunteerId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<VolunteerSignups>()
				.HasOne(x => x.Event)
				.WithMany(e => e.VolunteerSignups)
				.HasForeignKey(x => x.EventId)
				.OnDelete(DeleteBehavior.Restrict);

			#endregion

			#region StatusHistory
			modelBuilder.Entity<StatusHistory>()
				.HasKey(c => c.StatusHistoryId);

			modelBuilder.Entity<StatusHistory>()
				.HasOne(x => x.Event)
				.WithMany(x => x.StatusHistories)
				.HasForeignKey(x => x.EventId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<StatusHistory>()
				.HasOne(x => x.User)
				.WithMany(x => x.StatusHistories)
				.HasForeignKey(x => x.ChangedBy)
				.OnDelete(DeleteBehavior.Restrict);


			#endregion

			#region Events
			modelBuilder.Entity<Event>()
				.HasKey(c => c.EventId);

			modelBuilder.Entity<Event>()
				.HasOne(x => x.Category)
				.WithMany(x => x.Events)
				.HasForeignKey(x => x.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<Event>()
				.HasOne(x => x.Location)
				.WithMany(x => x.Events)
				.HasForeignKey(x => x.LocationId)
				.OnDelete(DeleteBehavior.Restrict);



			modelBuilder.Entity<Event>()
				.HasOne(x => x.User)
				.WithMany(u => u.Events)
				.HasForeignKey(x => x.CreatedBy)
				.OnDelete(DeleteBehavior.Restrict);

			#endregion

		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.UseLazyLoadingProxies();

			/*if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseLazyLoadingProxies();
			}*/
		}


	}
}

