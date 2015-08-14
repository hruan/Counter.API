using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Counter.Api.Core.Extensions
{
    public static class TaskExtensions
    {
        public static ConfiguredTaskAwaitable ContinueOnExecutionContext(this Task task)
        {
            return task.ConfigureAwait(false);
        }

        public static ConfiguredTaskAwaitable<T> ContinueOnExecutionContext<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false);
        }
    }
}