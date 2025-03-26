using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelApplication.Data;
using HotelApplication.Models;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        
        builder.Services.AddDbContext<HotelApplicationContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<HotelApplicationContext>()
            .AddDefaultTokenProviders();

        
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            try
            {
                await DbInitializer.SeedRolesAndUsersAsync(roleManager, userManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Errore durante il seeding degli utenti/ruoli");
            }
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
              name: "default",
              pattern: "{controller=Home}/{action=Index}/{id?}");

        //app.MapControllerRoute(
        //    name: "prenotazioni",
        //    pattern: "Prenotazioni/{action=Index}/{id?}",
        //    defaults: new { controller = "Prenotazioni" });

        //app.MapControllerRoute(
        //    name: "account",
        //    pattern: "Account/{action=Login}/{id?}",
        //    defaults: new { controller = "Account" });




        app.Run();
    }
}
