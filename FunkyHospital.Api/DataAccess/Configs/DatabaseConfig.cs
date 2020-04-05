using Hatan.Azure.Functions.DependencyInjection.Extensions;

namespace FunkyHospital.Api.DataAccess.Configs
{
    public class DatabaseConfig : ICustomApplicationSetting
    {
        public string TableName { get; set; }
        public string ConnectionString { get; set; }
    }
}