using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelApplication.Data;
using HotelApplication.Models;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configurazione del database
        builder.Services.AddDbContext<HotelApplicationContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Configurazione Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<HotelApplicationContext>()
            .AddDefaultTokenProviders();

        // Configurazione dei cookie per l'autenticazione
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Seeding automatico degli utenti e ruoli
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

        // Configurazione di middleware e rotte
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



        app.Run();
    }
}
