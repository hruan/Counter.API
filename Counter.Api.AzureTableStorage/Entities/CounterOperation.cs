using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Counter.Api.AzureTableStorage.Entities
{
    public class CounterOperation : TableEntity
    {
        // Required by table storage ...
        public CounterOperation() { }

        public CounterOperation(string partitionKey, int count)
        {
            PartitionKey = partitionKey;
            RowKey = DateTime.UtcNow.ToString("O");
            Count = count;
        }

        public int Count { get; set; }
    }
}