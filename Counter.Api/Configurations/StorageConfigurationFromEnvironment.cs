using System;
using Counter.Api.Core.Contracts;

namespace Counter.Api.Configurations
{
    public class StorageConfigurationFromEnvironment : IStorageConfiguration
    {
        public string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable("StorageConnectionString");
        }

        public string GetContainer()
        {
            return Environment.GetEnvironmentVariable("StorageContainerName");
        }
    }
}