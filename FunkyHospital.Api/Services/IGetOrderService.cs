using System.Collections.Generic;
using System.Threading.Tasks;
using FunkyHospital.Api.Core;
using FunkyHospital.Api.DTO;
using FunkyHospital.Api.Models;

namespace FunkyHospital.Api.Services
{
    public interface IGetOrderService
    {
        Task<Result<List<DisplayOrderDto>>> GetOrderAsync(GetOrderRequest request);
    }
}