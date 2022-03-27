using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reservation.Data;
using Reservation.Service.Helpers;
using System.Globalization;

namespace Reservation.ServiceMember
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.RegisterApplicationServices();
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
					.AddCookie(options =>
					{
						options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/ServiceMember/VerifyServiceMember");
					});
			services.AddDbContext<ApplicationContext>(
				options => options.UseSqlServer(Configuration.GetConnectionString("LocalConnection")));
			services.AddLocalization(opts =>
			{
				opts.ResourcesPath = "Resources";
			});
			services.AddControllersWithViews();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			var supportedCultures = new[]
			{
				new CultureInfo("en"),
				new CultureInfo("ru"),
				new CultureInfo("hy"),
				new CultureInfo("en-US"),
				new CultureInfo("ru-RU"),
				new CultureInfo("hy-AM")
			};
			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture("en-US"),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			});

			app.UseRequestLocalization();
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
		}
	}
}
