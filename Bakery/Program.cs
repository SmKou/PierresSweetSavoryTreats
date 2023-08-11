global using System;
global using System.Collections.Generic;
global using System.Linq;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Bakery.Models;

namespace Bakery;

class Program
{
    static void Main(string[] args)
    {

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<BakeryContext>(
            dbContextOptions => dbContextOptions
            .UseMySql(
                builder.Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
            )
            )
        );

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<BakeryContext>();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            /*  Default password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            */
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 0;
            options.Password.RequiredUniqueChars = 0;
        });

        WebApplication app = builder.Build();

        // app.UseDeveloperExceptionPage();
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