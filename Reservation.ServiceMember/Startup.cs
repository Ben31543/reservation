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
using System.Linq;
using Reservation.Resources;
using Reservation.Resources.Constants;

namespace Reservation.ServiceMember
{
	public class Startup
	{
		private readonly string _allowAngularPolicy = "allowAngular";
		
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.RegisterApplicationServices();
			
			services.AddCors(options =>
			{
				var allowedCorsOrigins = Configuration.GetValue<string>("AppSettings:AllowedCorsOrigins").Split(new char[] { ',' });
				options.AddPolicy(
					_allowAngularPolicy,
					builder =>
					{
						builder
							.WithOrigins(allowedCorsOrigins)
							.AllowAnyHeader()
							.WithMethods("GET", "POST", "OPTIONS", "DELETE", "PUT")
							.AllowCredentials();
					});
			});
			
			services.AddImagesSavingService(Configuration);
			services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TestDbConnection")));
			services.AddControllersWithViews();
			services.AddLocalization(opts =>
			{
				opts.ResourcesPath = "Resources";
			});

			services.AddHttpContextAccessor();
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

			var supportedCultures = CommonConstants.SupportedLanguages.Select(culture => new CultureInfo(culture)).ToArray();
			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture("en-US"),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			});

			app.UseCors(_allowAngularPolicy);
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
