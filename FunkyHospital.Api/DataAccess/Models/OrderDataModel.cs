using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunkyHospital.Api.DataAccess.Models
{
    public class OrderDataModel : TableEntity
    {
        public string OrderId { get; set; }
        public string OrderDateUtc { get; set; }

        public string Name { get; set; }
        public string PostCode { get; set; }
        public string MobileNumber { get; set; }
    }
}
