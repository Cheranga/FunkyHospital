using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FunkyHospital.Api.DataAccess.CommandHandlers;
using FunkyHospital.Api.DataAccess.Commands;
using FunkyHospital.Api.DataAccess.Models;
using FunkyHospital.Api.DTO;
using FunkyHospital.Api.Mappers;
using FunkyHospital.Api.Models;
using FunkyHospital.Api.Tests.DataAccess.Util;
using FunkyHospital.Api.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Xunit;

namespace FunkyHospital.Api.Tests.DataAccess
{
    public class CreateOrderCommandHandlerTests : IDisposable
    {
        private DatabaseConfig _databaseConfig;
        private readonly TestDataManager<OrderDataModel> _testDataManager;

        public CreateOrderCommandHandlerTests()
        {
            _databaseConfig = new DatabaseConfig
            {
                ConnectionString = "UseDevelopmentStorage=true",
                TableName = "TestNewOrders"
            };

            _testDataManager = new TestDataManager<OrderDataModel>(_databaseConfig);
        }

        [Fact]
        public async Task Test()
        {
            

            var createOrderRequest = new CreateOrderRequest
            {
                OrderId = Guid.NewGuid().ToString("N"),
                Name = "John Snow",
                MobileNumber = "0410123456",
                PostCode = "0001"
            };

            var validator = new CreateOrderCommandValidator();

            var mappingConfig = new MapperConfiguration(expression => { expression.AddProfile<MapperProfile>(); });
            var mapper = mappingConfig.CreateMapper();

            var command = mapper.Map<CreateOrderCommand>(createOrderRequest);

            var commandHandler = new CreateOrderCommandHandler(_databaseConfig, validator, mapper, Mock.Of<ILogger<CreateOrderCommandHandler>>());
            var operation = await commandHandler.ExecuteAsync(command);

            Assert.NotNull(operation);
            Assert.True(operation.Status);

            var orderModel = mapper.Map<OrderDataModel>(command);
            var results = await _testDataManager.GetAsync(orderModel.PartitionKey, orderModel.RowKey);

            Assert.Single(results);
        }

        public void Dispose()
        {
            _testDataManager.Dispose();
        }
    }
}
