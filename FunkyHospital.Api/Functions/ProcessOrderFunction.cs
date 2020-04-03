using System;
using System.Threading.Tasks;
using FunkyHospital.Api.Models;
using FunkyHospital.Api.Services;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunkyHospital.Api.Functions
{
    public class ProcessOrderFunction
    {
        private readonly ICreateOrderService _service;
        private readonly ILogger<ProcessOrderFunction> _logger;

        public ProcessOrderFunction(ICreateOrderService service, ILogger<ProcessOrderFunction> logger)
        {
            _service = service;
            _logger = logger;
        }

        [FunctionName(nameof(ProcessOrderFunction))]
        public async Task RunAsync([ServiceBusTrigger("%NewOrdersQueue%")]CreateOrderRequest message)
        {

        }
    }
}
