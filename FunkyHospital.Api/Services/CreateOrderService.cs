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
    public class CreateOrderService : ICreateOrderService
    {
        private readonly ICommandHandler<CreateOrderCommand> _commandHandler;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderService> _logger;

        public CreateOrderService(ICommandHandler<CreateOrderCommand> commandHandler, IMapper mapper, ILogger<CreateOrderService> logger)
        {
            _commandHandler = commandHandler;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result> CreateOrderAsync(CreateOrderRequest request)
        {
            try
            {
                _logger.LogInformation($"{nameof(CreateOrderService)} started.");

                var createOrderCommand = _mapper.Map<CreateOrderCommand>(request);

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
                _logger.LogInformation($"{nameof(CreateOrderService)} finished.");
            }
        }
    }
}