using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using FluentValidation;
using FunkyHospital.Api.DTO;
using FunkyHospital.Api.Models;
using FunkyHospital.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunkyHospital.Api.Functions
{
    public class CreateOrderApiFunction
    {
        private readonly IValidator<CreateOrderDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderApiFunction> _logger;

        public CreateOrderApiFunction(IValidator<CreateOrderDto> validator, IMapper mapper, ILogger<CreateOrderApiFunction> logger)
        {
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }
        
        [FunctionName(nameof(CreateOrderApiFunction))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders/create")]
            HttpRequest request,
            [ServiceBus("%NewOrdersQueue%")]IAsyncCollector<CreateOrderDto> orders)
        {
            try
            {
                var orderDto = _mapper.Map<CreateOrderDto>(request);
                var validationResult = await _validator.ValidateAsync(orderDto);

                if (!validationResult.IsValid)
                {
                    return new BadRequestObjectResult("Invalid request.");
                }


                await orders.AddAsync(orderDto).ConfigureAwait(false);

                return new OkObjectResult(orderDto.OrderId);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error occured when creating the medicine order\n{exception.StackTrace}");
            }

            return new InternalServerErrorResult();
        }
    }
}
