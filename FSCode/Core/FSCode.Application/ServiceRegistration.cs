using AutoMapper;
using FluentValidation.AspNetCore;
using FSCode.Application.Profiles;
using FSCode.Application.Validators.ReminderValidators;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddFluentValidation(x => x.RegisterValidatorsFromAssembly(typeof(ReminderCreateDtoValidator).Assembly));
        } 
    }
}
