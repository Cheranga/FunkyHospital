using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FunkyHospital.Api.Core;
using FunkyHospital.Api.DataAccess.CommandHandlers;
using FunkyHospital.Api.DataAccess.Commands;
using FunkyHospital.Api.Models;
using Microsoft.Extensions.Logging;

namespace FunkyHospital.Api.Services
{
    public class ProcessPatientService : IProcessPatientService
    {
        private readonly ICommandHandler<EnrollPatientCommand> _commandHandler;
        private readonly IMapper _mapper;
        private readonly ILogger<ProcessPatientService> _logger;

        public ProcessPatientService(ICommandHandler<EnrollPatientCommand> commandHandler, IMapper mapper, ILogger<ProcessPatientService> logger)
        {
            _commandHandler = commandHandler;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result> CreateOrderAsync(CreateOrderRequest request)
        {
            try
            {
                _logger.LogInformation($"{nameof(ProcessPatientService)} started.");

                var createOrderCommand = _mapper.Map<EnrollPatientCommand>(request);

                var operation = await _commandHandler.ExecuteAsync(createOrderCommand).ConfigureAwait(false);

                if (!operation.Status)
                {
                    _logger.LogError("Create order failed.");
                    return Result.Failure("Create order failed.");
                }

                _logger.LogInformation("Create order success.");
                return Result.Success();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error occured when creating the order.");
                return Result.Failure("Error occured when creating the order.");
            }
            finally
            {
                _logger.LogInformation($"{nameof(ProcessPatientService)} finished.");
            }
        }
    }
}