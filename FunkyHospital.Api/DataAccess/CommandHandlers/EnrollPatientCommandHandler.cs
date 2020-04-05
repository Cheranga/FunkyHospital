using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FunkyHospital.Api.Core;
using FunkyHospital.Api.DataAccess.Commands;
using FunkyHospital.Api.DataAccess.Configs;
using FunkyHospital.Api.DataAccess.Models;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunkyHospital.Api.DataAccess.CommandHandlers
{
    public class EnrollPatientCommandHandler : CommandHandlerBase<EnrollPatientCommand>
    {
        private readonly IValidator<EnrollPatientCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<EnrollPatientCommandHandler> _logger;

        public EnrollPatientCommandHandler(DatabaseConfig databaseConfig,
            IValidator<EnrollPatientCommand> validator,
            IMapper mapper,
            ILogger<EnrollPatientCommandHandler> logger) : base(databaseConfig)
        {
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task<Result> ExecuteAsync(EnrollPatientCommand command)
        {
            try
            {
                _logger.LogInformation($"{nameof(EnrollPatientCommandHandler)} started.");

                var validationResult = await _validator.ValidateAsync(command).ConfigureAwait(false);

                if (!validationResult.IsValid)
                {
                    _logger.LogError($"Invalid create order command: {validationResult.Errors.First().ErrorMessage}");
                    return Result.Failure("Invalid create order command.");
                }

                var dataModel = _mapper.Map<OrderDataModel>(command);

                var tableOperation = TableOperation.InsertOrReplace(dataModel);

                await Table.ExecuteAsync(tableOperation).ConfigureAwait(false);

                _logger.LogInformation("Successfully created order.");

                return Result.Success();

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occured when creating an order.");
                return Result.Failure("Error occured when creating an order.");
            }
            finally
            {
                _logger.LogInformation($"{nameof(EnrollPatientCommandHandler)} finished.");
            }
        }
    }
}