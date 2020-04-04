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
using TestStack.BDDfy;
using Xunit;
using Result = FunkyHospital.Api.Core.Result;

namespace FunkyHospital.Api.Tests.DataAccess
{
    [Story(AsA = "CreateOrderCommandHandler",
        IWant = "to save valid orders in the table storage",
        SoThat = "they can be used for processing later.")]
    public class CreateOrderCommandHandlerTests : IDisposable
    {
        private DatabaseConfig _databaseConfig;
        private readonly TestDataManager<OrderDataModel> _testDataManager;
        private CreateOrderCommandValidator _validator;
        private IMapper _mapper;
        private CreateOrderRequest _createOrderRequest;
        private Result _operation;
        private CreateOrderCommand _command;
        private Mock<ILogger<CreateOrderCommandHandler>> _mockedLogger;

        public CreateOrderCommandHandlerTests()
        {
            _databaseConfig = new DatabaseConfig
            {
                ConnectionString = "UseDevelopmentStorage=true",
                TableName = "TestNewOrders"
            };

            _testDataManager = new TestDataManager<OrderDataModel>(_databaseConfig);
            _validator = new CreateOrderCommandValidator();

            var mappingConfig = new MapperConfiguration(expression => { expression.AddProfile<MapperProfile>(); });
            _mapper = mappingConfig.CreateMapper();

            _mockedLogger = new Mock<ILogger<CreateOrderCommandHandler>>();

        }

        [Fact]
        public Task ValidOrder()
        {
            this.Given(x => GivenItsAValidOrder())
                .When(x => WhenCommandIsExecuted())
                .Then(x => ThenTheOrderMustBeSuccessfullySavedInTheTable())
                .BDDfy();

            return Task.CompletedTask;

            //_createOrderRequest = new CreateOrderRequest
            //{
            //    OrderId = Guid.NewGuid().ToString("N"),
            //    Name = "John Snow",
            //    MobileNumber = "0410123456",
            //    PostCode = "0001"
            //};

            //_command = _mapper.Map<CreateOrderCommand>(_createOrderRequest);
            //var commandHandler = new CreateOrderCommandHandler(_databaseConfig, _validator, _mapper, Mock.Of<ILogger<CreateOrderCommandHandler>>());
            //var operation = await commandHandler.ExecuteAsync(_command);

            //Assert.NotNull(operation);
            //Assert.True(operation.Status);

            //var orderModel = _mapper.Map<OrderDataModel>(_command);
            //var results = await _testDataManager.GetAsync(orderModel.PartitionKey, orderModel.RowKey);

            //Assert.Single(results);
        }

        [Fact]
        public Task OrderWithNoName()
        {
            this.Given(x => GivenNameIsNotProvided())
                .When(x => WhenCommandIsExecuted())
                .Then(x => ThenTheOrderIsNotSavedInTheTable())
                .BDDfy();

            return Task.CompletedTask;
        }

        [Fact]
        public Task OrderWithNoMobile()
        {
            this.Given(x => GivenMobileIsNotProvided())
                .When(x => WhenCommandIsExecuted())
                .Then(x => ThenTheOrderIsNotSavedInTheTable())
                .BDDfy();

            return Task.CompletedTask;
        }

        [Fact]
        public Task OrderWithNoPostcode()
        {
            this.Given(x => GivenPostcodeIsNotProvided())
                .When(x => WhenCommandIsExecuted())
                .Then(x => ThenTheOrderIsNotSavedInTheTable())
                .BDDfy();

            return Task.CompletedTask;
        }

        private Task GivenNameIsNotProvided()
        {
            _createOrderRequest = new CreateOrderRequest
            {
                OrderId = Guid.NewGuid().ToString("N"),
                Name = string.Empty,
                MobileNumber = "0410123456",
                PostCode = "0001"
            };

            return Task.CompletedTask;
        }

        private Task GivenMobileIsNotProvided()
        {
            _createOrderRequest = new CreateOrderRequest
            {
                OrderId = Guid.NewGuid().ToString("N"),
                Name = "John Snow",
                MobileNumber = string.Empty,
                PostCode = "0001"
            };

            return Task.CompletedTask;
        }

        private Task GivenPostcodeIsNotProvided()
        {
            _createOrderRequest = new CreateOrderRequest
            {
                OrderId = Guid.NewGuid().ToString("N"),
                Name = "John Snow",
                MobileNumber = "0410123456",
                PostCode = string.Empty
            };

            return Task.CompletedTask;
        }

        private Task GivenItsAValidOrder()
        {
            _createOrderRequest = new CreateOrderRequest
            {
                OrderId = Guid.NewGuid().ToString("N"),
                Name = "John Snow",
                MobileNumber = "0410123456",
                PostCode = "0001"
            };

            return Task.CompletedTask;
        }

        private async Task WhenCommandIsExecuted()
        {
            _command = _mapper.Map<CreateOrderCommand>(_createOrderRequest);
            var commandHandler = new CreateOrderCommandHandler(_databaseConfig, _validator, _mapper, _mockedLogger.Object);
            _operation = await commandHandler.ExecuteAsync(_command);
        }

        private async Task ThenTheOrderMustBeSuccessfullySavedInTheTable()
        {
            Assert.NotNull(_operation);
            Assert.True(_operation.Status);

            var orderModel = _mapper.Map<OrderDataModel>(_command);
            var results = await _testDataManager.GetAsync(orderModel.PartitionKey, orderModel.RowKey);

            Assert.Single(results);
        }

        private Task ThenTheOrderIsNotSavedInTheTable()
        {
            Assert.NotNull(_operation);
            Assert.False(_operation.Status);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _testDataManager.Dispose();
        }
    }
}
