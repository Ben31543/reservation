using Microsoft.Extensions.DependencyInjection;
using Reservation.Service.Interfaces;
using Reservation.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
