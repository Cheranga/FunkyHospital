using System.Threading.Tasks;
using FunkyHospital.Api.Core;
using FunkyHospital.Api.DataAccess.Configs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunkyHospital.Api.DataAccess.CommandHandlers
{
    public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand> where TCommand:ICommand
    {
        protected CloudTable Table;

        protected CommandHandlerBase(DatabaseConfig databaseConfig)
        {
            var storageAccount = CloudStorageAccount.Parse(databaseConfig.ConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient();

            Table = tableClient.GetTableReference(databaseConfig.TableName);
            Table.CreateIfNotExistsAsync().Wait();
        }

        public abstract Task<Result> ExecuteAsync(TCommand command);
    }
}