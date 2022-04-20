using Microsoft.Extensions.DependencyInjection;
using Reservation.Service.Interfaces;
using Reservation.Service.Services;
using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace Reservation.Service.Helpers
{
    public static class ServiceExtensions
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IServiceMemberService, ServiceMemberService>();
            services.AddScoped<IServiceMemberBranchService, ServiceMemberBranchService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IBankCardService, BankCardService>();
            services.AddScoped<IReservingService, ReservingService>();
            services.AddScoped<IPaymentService, PaymentService>();
        }
        
        public static void AddImagesSavingService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IImageSavingService, ImageSavingService>(
                client =>
                {
                    client.BaseAddress = new Uri(configuration.GetSection("CSEventLoggerSettings:CsEventLoggerHost").Value);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                });
        }
    }
}
