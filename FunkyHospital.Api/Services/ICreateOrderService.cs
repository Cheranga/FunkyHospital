using System.Threading.Tasks;
using FunkyHospital.Api.Core;
using FunkyHospital.Api.Models;

namespace FunkyHospital.Api.Services
{
    public interface ICreateOrderService
    {
        Task<Result> CreateOrderAsync(CreateOrderRequest request);
    }
}