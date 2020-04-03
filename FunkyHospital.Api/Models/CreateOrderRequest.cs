using System;

namespace FunkyHospital.Api.Models
{
    public class CreateOrderRequest
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString("N");
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Name { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
    }
}