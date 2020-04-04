using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FunkyHospital.Api.DataAccess.CommandHandlers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunkyHospital.Api.Tests.DataAccess.Util
{
    public class TestDataManager<TEntity> : IDisposable where TEntity : TableEntity, new()
    {
        private readonly CloudTable _table;

        public TestDataManager(DatabaseConfig databaseConfig)
        {
            var storageAccount = CloudStorageAccount.Parse(databaseConfig.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();

            _table = tableClient.GetTableReference(databaseConfig.TableName);
            _table.CreateIfNotExistsAsync().Wait();

        }

        public async Task UpsertAsync(params TEntity[] entities)
        {
            if (entities == null)
            {
                return;
            }

            await _table.CreateIfNotExistsAsync();

            foreach (var entity in entities)
            {
                await _table.ExecuteAsync(TableOperation.InsertOrReplace(entity)).ConfigureAwait(false);
            }
        }

        public async Task<List<TEntity>> GetAsync(string partitionKey, string rowId)
        {
            var partitionKeyQuery = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
            var rowIdQuery = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowId);
            var combinedQuery = TableQuery.CombineFilters(partitionKeyQuery, TableOperators.And, rowIdQuery);
            var query = new TableQuery<TEntity>().Where(combinedQuery);

            var operation = await _table.ExecuteQuerySegmentedAsync(query, new TableContinuationToken());
            return operation.Results;
        }

        public void Dispose()
        {
            _table.DeleteIfExistsAsync().Wait();
        }

    }
}
