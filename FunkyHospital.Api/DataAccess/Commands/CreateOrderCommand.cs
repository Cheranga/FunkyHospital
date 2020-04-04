using System;
using System.Collections.Generic;
using System.Text;
using FunkyHospital.Api.DataAccess.CommandHandlers;

namespace FunkyHospital.Api.DataAccess.Commands
{
    public class CreateOrderCommand : ICommand
    {
        public string OrderId { get; set; }
        public string Name { get; set; }
        public string PostCode { get; set; }
        public string MobileNumber { get; set; }
    }
}
