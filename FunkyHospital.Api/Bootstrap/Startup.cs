using AutoMapper;
using FluentValidation;
using FunkyHospital.Api.Bootstrap;
using FunkyHospital.Api.DTO;
using FunkyHospital.Api.Mappers;
using FunkyHospital.Api.Services;
using FunkyHospital.Api.Validators;
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
        }

        private void RegisterServices(IServiceCollection services)
        {
            if (services == null)
            {
                return;
            }

            services.AddSingleton<ICreateOrderService, CreateOrderService>();
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