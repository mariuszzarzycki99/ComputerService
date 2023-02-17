using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TabApp.Enums;
using Microsoft.EntityFrameworkCore;

namespace TabApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
                    {
                        options.LoginPath = "/login";
                        options.AccessDeniedPath = "/denied";
                    });
            builder.Services.AddDbContext<dbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("dbContext"));
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.EmployeePolicy, policy =>
                    policy.RequireRole(Roles.Support, Roles.Employee, Roles.Manager, Roles.Admin));
                options.AddPolicy(Policies.ManagerPolicy, policy =>
                    policy.RequireRole(Roles.Manager, Roles.Admin));
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}
