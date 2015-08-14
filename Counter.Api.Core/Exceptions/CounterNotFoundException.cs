using System;

namespace Counter.Api.Core.Exceptions
{
    public class CounterNotFoundException : Exception
    {
        public CounterNotFoundException(string counterId, Exception innerException)
            : base("Counter not found", innerException)
        {
            CounterId = counterId;
        }

        public string CounterId { get; private set; }
    }
}