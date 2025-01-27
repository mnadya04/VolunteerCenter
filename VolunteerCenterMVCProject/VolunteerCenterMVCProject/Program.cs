
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VolunteerCenterMVCProject.Data;
using VolunteerCenterMVCProject.Models;
using VolunteerCenterMVCProject.Services.Interfaces;
using VolunteerCenterMVCProject.Services;
using HousekeeperApp.Data.Seeding;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddSession();


builder.Services.ConfigureApplicationCookie(options =>
{   
    options.Cookie.HttpOnly = true;
    //options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
    options.SlidingExpiration = false;
});


// Identity configuration
builder.Services.AddDefaultIdentity<User>()
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();

// Add MVC and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register services
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<ICategoriesService, CategoriesService>();
builder.Services.AddTransient<IEventsService, EventsService>();
//IServiceCollection serviceCollection = builder.Services.AddTransient<ICategoriesService, CategoriesService>();
//IServiceCollection serviceCollection2 = builder.Services.AddTransient<IEventsService, EventsService>();
builder.Services.AddTransient<IStatusHistoryService, StatusHistoryService>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddScoped<ISignUpsService, SignUpsService>();


var app = builder.Build();

// Seed data 
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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


