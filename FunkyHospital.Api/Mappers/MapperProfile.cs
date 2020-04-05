using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FunkyHospital.Api.DataAccess.Commands;
using FunkyHospital.Api.DataAccess.Models;
using FunkyHospital.Api.DTO;
using FunkyHospital.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FunkyHospital.Api.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateOrderDto, CreateOrderRequest>();
            CreateMap<GetOrderDto, GetOrderRequest>();
            CreateMap<CreateOrderRequest, EnrollPatientCommand>();
            CreateMap<EnrollPatientCommand, OrderDataModel>().AfterMap((source, target) =>
            {
                var partitionKey = source.PostCode.ToUpper();

                target.PartitionKey = partitionKey;
                target.RowKey = source.OrderId.ToUpper();
                target.OrderId = $"{partitionKey}:{source.OrderId}";
                target.OrderDateUtc = DateTime.UtcNow.ToString();
            });

            CreateMap<HttpRequest, CreateOrderDto>().ConvertUsing<CreateOrderMapper>();
        }
    }
}
