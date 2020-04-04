using System;

namespace FunkyHospital.Api.Models
{
    public class CreateOrderRequest
    {   
        public string Name { get; set; }
        public string PostCode { get; set; }
        public string MobileNumber { get; set; }

        public string OrderId { get; set; }
    }
}