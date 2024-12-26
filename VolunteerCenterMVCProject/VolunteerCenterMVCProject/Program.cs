
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VolunteerCenterMVCProject.Data;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.Services;
using Microsoft.Extensions.DependencyInjection;
using static System.Formats.Asn1.AsnWriter;
using HousekeeperApp.Data.Seeding;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity configuration
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Add MVC and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register services
builder.Services.AddTransient<IUsersService, UsersService>();


var app = builder.Build();

// Seed data on application startup
if (app.Environment.IsDevelopment())
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		dbContext.Database.Migrate();

		var serviceProvider = serviceScope.ServiceProvider;

        var seedData = new SeedData();
        await seedData.SeedAsync(dbContext, serviceProvider);
	}

    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Configure middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


