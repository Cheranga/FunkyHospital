using System.Collections.Generic;
using System.Threading.Tasks;
using FunkyHospital.Api.Core;
using FunkyHospital.Api.DTO;
using FunkyHospital.Api.Models;

namespace FunkyHospital.Api.Services
{
    public class GetOrderService : IGetOrderService
    {
        public Task<Result<List<DisplayOrderDto>>> GetOrderAsync(GetOrderRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}