using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.ViewModels.Categories;

namespace VolunteerCenterMVCProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<VolunteerSignups> Signups { get; set; }
        public DbSet<StatusHistory> statusHistories { get; set; }
        public DbSet<Event> Events { get; set; }


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
                .HasForeignKey(x => x.VolunteerId);

            modelBuilder.Entity<VolunteerSignups>()
                .HasOne(x => x.Event)
                .WithMany(e => e.VolunteerSignups)
                .HasForeignKey(x => x.EventId);

            #endregion

            #region StatusHistory
            modelBuilder.Entity<StatusHistory>()
                .HasKey(c => c.StatusHistoryId);

            modelBuilder.Entity<StatusHistory>()
                .HasOne(x => x.Event)
                .WithMany(x => x.StatusHistories)
                .HasForeignKey(x => x.EventId);

            modelBuilder.Entity<StatusHistory>()
                .HasOne(x => x.User)
                .WithMany(x => x.StatusHistories)
                .HasForeignKey(x => x.ChangedBy);

            #endregion

            #region Events
            modelBuilder.Entity<Event>()
                .HasKey(c => c.EventId);

            modelBuilder.Entity<Event>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.CategoryId);

            modelBuilder.Entity<Event>()
                .HasOne(x => x.Location)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.LocationId);



            modelBuilder.Entity<Event>()
                .HasOne(x => x.User)
                .WithMany(u => u.Events)
                .HasForeignKey(x => x.CreatedBy);
            #endregion

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }


    }
}