using System;
using Counter.Api.Core.Contracts;

namespace Counter.Api.Configurations
{
    public class ApplicationConfigurationFromEnvironment : IApplicationConfiguration
    {
        public string GetAllowedOrigins()
        {
            return Environment.GetEnvironmentVariable("AllowedOrigins");
        }
    }
}