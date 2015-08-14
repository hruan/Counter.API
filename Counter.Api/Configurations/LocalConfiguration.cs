using System;
using System.Collections.Generic;

namespace Counter.Api.Configurations
{
    public static class LocalConfiguration
    {
        public static void SetEnvironmentVariables()
        {
            var vars = new Dictionary<string, string>
            {
                {"StorageConnectionString", "UseDevelopmentStorage=true"},
                {"StorageContainerName", "counters"},
                {"AllowedOrigins", "*"},
            };

            foreach (var kvp in vars)
            {
                Environment.SetEnvironmentVariable(kvp.Key, kvp.Value);
            }
        }
    }
}