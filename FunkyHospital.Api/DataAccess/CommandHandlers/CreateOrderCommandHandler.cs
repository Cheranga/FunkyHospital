using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FunkyHospital.Api.Core;
using FunkyHospital.Api.DataAccess.Commands;
using FunkyHospital.Api.DataAccess.Models;
using Hatan.Azure.Functions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunkyHospital.Api.DataAccess.CommandHandlers
{
    public class DatabaseConfig : ICustomApplicationSetting
    {
        public string TableName { get; set; }
        public string ConnectionString { get; set; }
    }

    public interface ICommand
    {
    }

    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task<Result> ExecuteAsync(TCommand command);
    }

    public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand> where TCommand:ICommand
    {
        //protected readonly CloudTableClient TableClient;
        //protected readonly DatabaseConfig DatabaseConfig;
        protected CloudTable Table;

        protected CommandHandlerBase(DatabaseConfig databaseConfig)
        {
            var storageAccount = CloudStorageAccount.Parse(databaseConfig.ConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient();

            Table = tableClient.GetTableReference(databaseConfig.TableName);
            Table.CreateIfNotExistsAsync().Wait();
        }

        public abstract Task<Result> ExecuteAsync(TCommand command);
    }

    public class CreateOrderCommandHandler : CommandHandlerBase<CreateOrderCommand>
    {
        private readonly IValidator<CreateOrderCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(DatabaseConfig databaseConfig,
            IValidator<CreateOrderCommand> validator,
            IMapper mapper,
            ILogger<CreateOrderCommandHandler> logger) : base(databaseConfig)
        {
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task<Result> ExecuteAsync(CreateOrderCommand command)
        {
            try
            {
                _logger.LogInformation($"{nameof(CreateOrderCommandHandler)} started.");

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
                _logger.LogInformation($"{nameof(CreateOrderCommandHandler)} finished.");
            }
        }
    }
}