using System;

namespace FunkyHospital.Api.DTO
{
    public class CreateOrderDto
    {
        public string Name { get; set; }
        public string PostCode { get; set; }
        public string MobileNumber { get; set; }

        public string OrderId { get; } = Guid.NewGuid().ToString("N");
    }
}