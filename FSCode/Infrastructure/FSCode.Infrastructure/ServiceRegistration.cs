using FSCode.Application.Services.EmailServices;
using FSCode.Application.Services.TelegramServices;
using FSCode.Infrastructure.Services.EmailServices;
using FSCode.Infrastructure.Services.TelegramServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITelegramService, TelegramService>();
        }
    }
}
