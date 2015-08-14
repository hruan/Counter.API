using System;
using System.Linq;
using System.Threading.Tasks;
using Counter.Api.AzureTableStorage.Entities;
using Counter.Api.Core.Contracts;
using Counter.Api.Core.Extensions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;

namespace Counter.Api.AzureTableStorage
{
    public class AzureTableStorageCounter : IPersistingCounter
    {
        private const string IncrementKey = "INC";
        private const string DecrementKey = "DEC";

        private readonly CloudTable _table;

        public AzureTableStorageCounter(IStorageConfiguration configuration)
        {
            var account = CloudStorageAccount.Parse(configuration.GetConnectionString());
            var client = account.CreateCloudTableClient();
            _table = client.GetTableReference(configuration.GetContainer());
        }

        public Task IncrementAsync(string counter, int amount)
        {
            var entity = new CounterOperation(PartitionKeyFor(counter, IncrementKey), amount);
            var op = TableOperation.Insert(entity);
            return _table.ExecuteAsync(op);
        }

        public Task DecrementAsync(string counter, int amount)
        {
            var entity = new CounterOperation(PartitionKeyFor(counter, DecrementKey), amount);
            var op = TableOperation.Insert(entity);
            return _table.ExecuteAsync(op);
        }

        public async Task<long> GetCurrentCountAsync(string counter)
        {
            var incOp = _table.CreateQuery<CounterOperation>()
                .Where(x => x.PartitionKey == PartitionKeyFor(counter, IncrementKey))
                .AsTableQuery();
            var decOp = _table.CreateQuery<CounterOperation>()
                .Where(x => x.PartitionKey == PartitionKeyFor(counter, DecrementKey))
                .AsTableQuery();
            var counts = await Task.WhenAll(CountAsync(incOp), CountAsync(decOp))
                .ContinueOnExecutionContext();

            return counts[0] - counts[1];
        }

        public async Task<long> CountAsync(TableQuery<CounterOperation> query)
        {
            TableContinuationToken token = null;
            long total = 0;
            do
            {
                var segment = await _table.ExecuteQuerySegmentedAsync(query, token).ContinueOnExecutionContext();
                total += segment.Aggregate(0, (x, op) => x + op.Count);
                token = segment.ContinuationToken;
            } while (token != null);

            return total;
        }

        private static string PartitionKeyFor(string counterId, string action)
        {
            var now = DateTime.UtcNow;
            return String.Join("-", counterId, action, now.ToString("yy-MM-dd"));
        }
    }
}
