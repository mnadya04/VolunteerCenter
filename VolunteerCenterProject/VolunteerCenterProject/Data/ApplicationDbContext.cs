using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection.Emit;
using VolunteerCenterProject.Models;

namespace VolunteerCenterProject.Data
{
	public class ApplicationDbContext : IdentityDbContext<Users, IdentityRole, string>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public  DbSet<Locations> Locations { get; set; }
		public  DbSet<Categories> Categories { get; set; }
		public  DbSet<VolunteerSignups> Signups { get; set; }
		//public  DbSet<Users> Users { get; set; } -- removed because IdentityDbContext<Users.. will set up 
		public DbSet<StatusHistory> statusHistories { get; set; }
		public  DbSet<Events> Events { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); 

			#region Categories
			modelBuilder.Entity<Categories>()
				.HasKey(x => x.CategoryId);
			#endregion

			#region Locations
			modelBuilder.Entity<Locations>()
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
			modelBuilder.Entity<Events>()
				.HasKey(c => c.EventId);

			modelBuilder.Entity<Events>()
				.HasOne(x => x.Category)
				.WithMany(x => x.Events)
				.HasForeignKey(x => x.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<Events>()
				.HasOne(x => x.Location)
				.WithMany(x => x.Events)
				.HasForeignKey(x => x.LocationId)
				.OnDelete(DeleteBehavior.Restrict);



			modelBuilder.Entity<Events>()
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

