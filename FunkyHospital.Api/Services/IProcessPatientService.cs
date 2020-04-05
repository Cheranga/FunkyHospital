using System.Threading.Tasks;
using FunkyHospital.Api.Core;
using FunkyHospital.Api.Models;

namespace FunkyHospital.Api.Services
{
    public interface IProcessPatientService
    {
        Task<Result> CreateOrderAsync(CreateOrderRequest request);
    }
}