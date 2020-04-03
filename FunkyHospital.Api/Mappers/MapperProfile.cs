using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
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

            CreateMap<HttpRequest, CreateOrderDto>().ConvertUsing<CreateOrderMapper>();
        }
    }
}
