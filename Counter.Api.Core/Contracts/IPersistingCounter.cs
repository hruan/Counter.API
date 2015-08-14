using System.Threading.Tasks;

namespace Counter.Api.Core.Contracts
{
    public interface IPersistingCounter
    {
        Task IncrementAsync(string counter, int amount);
        Task DecrementAsync(string counter, int amount);
        Task<long> GetCurrentCountAsync(string counter);
    }
}