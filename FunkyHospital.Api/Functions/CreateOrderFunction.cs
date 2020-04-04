using System;
using System.Threading.Tasks;
using AutoMapper;
using FunkyHospital.Api.DTO;
using FunkyHospital.Api.Models;
using FunkyHospital.Api.Services;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunkyHospital.Api.Functions
{
    public class CreateOrderFunction
    {
        private readonly ICreateOrderService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderFunction> _logger;

        public CreateOrderFunction(ICreateOrderService service, IMapper mapper, ILogger<CreateOrderFunction> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [FunctionName(nameof(CreateOrderFunction))]
        public async Task RunAsync([ServiceBusTrigger("%NewOrdersQueue%")]CreateOrderDto dto)
        {
            var request = _mapper.Map<CreateOrderRequest>(dto);

            var operation = await _service.CreateOrderAsync(request).ConfigureAwait(false);

            if (!operation.Status)
            {
                _logger.LogError($"Crete order function failed.");
                return;
            }

            _logger.LogInformation($"Create order function successful.");
        }
    }
}
