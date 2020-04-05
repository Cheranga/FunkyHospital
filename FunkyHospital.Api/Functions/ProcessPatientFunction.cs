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
    public class ProcessPatientFunction
    {
        private readonly IProcessPatientService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<ProcessPatientFunction> _logger;

        public ProcessPatientFunction(IProcessPatientService service, IMapper mapper, ILogger<ProcessPatientFunction> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [FunctionName(nameof(ProcessPatientFunction))]
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
