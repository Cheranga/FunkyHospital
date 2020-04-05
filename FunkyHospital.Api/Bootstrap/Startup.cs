using AutoMapper;
using FluentValidation;
using FunkyHospital.Api.Bootstrap;
using FunkyHospital.Api.DataAccess.CommandHandlers;
using FunkyHospital.Api.DataAccess.Commands;
using FunkyHospital.Api.DataAccess.Configs;
using FunkyHospital.Api.DTO;
using FunkyHospital.Api.Mappers;
using FunkyHospital.Api.Services;
using FunkyHospital.Api.Validators;
using Hatan.Azure.Functions.DependencyInjection.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace FunkyHospital.Api.Bootstrap
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            RegisterValidators(services);
            RegisterMappers(services);
            RegisterServices(services);
            RegisterDataAccess(services);
            RegisterConfigurations(services);
        }

        private void RegisterDataAccess(IServiceCollection services)
        {
            if (services == null)
            {
                return;
            }

            services.AddSingleton<ICommandHandler<EnrollPatientCommand>, EnrollPatientCommandHandler>();
        }

        private void RegisterConfigurations(IServiceCollection services)
        {
            if (services == null)
            {
                return;
            }

            services.RegisterConfiguration<DatabaseConfig>(ServiceLifetime.Singleton);
        }

        private void RegisterServices(IServiceCollection services)
        {
            if (services == null)
            {
                return;
            }

            services.AddSingleton<IProcessPatientService, ProcessPatientService>();
            services.AddSingleton<IGetOrderService, GetOrderService>();
        }

        private void RegisterValidators(IServiceCollection services)
        {
            if (services == null)
            {
                return;
            }

            services.AddSingleton<IValidator<CreateOrderDto>, CreateOrderDtoValidator>();
            services.AddSingleton<IValidator<GetOrderDto>, GetOrderDtoValidator>();
            services.AddSingleton<IValidator<EnrollPatientCommand>, CreateOrderCommandValidator>();
        }

        private void RegisterMappers(IServiceCollection services)
        {
            if (services == null)
            {
                return;
            }

            var mappingConfig = new MapperConfiguration(expression => { expression.AddProfile<MapperProfile>(); });
            var mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}