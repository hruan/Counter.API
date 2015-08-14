using System;
using System.Threading.Tasks;
using System.Web.Http;
using Counter.Api.Core.Contracts;
using Counter.Api.Core.Exceptions;
using Counter.Api.Core.Extensions;

namespace Counter.Api.Controllers
{
    [RoutePrefix("api/count")]
    public class CountController : ApiController
    {
        private readonly IPersistingCounter _counter;

        public CountController(IPersistingCounter persistingCounter)
        {
            _counter = persistingCounter;
        }

        [HttpPost]
        [Route("{counter}")]
        public async Task<IHttpActionResult> Submit(string counter, [FromBody] string action)
        {
            if (String.IsNullOrEmpty(action))
            {
                return BadRequest("Action ({upvote, downvote}) must be be supplied");
            }

            try
            {
                var f = CounterAction(action);
                await f(counter).ContinueOnExecutionContext();
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Action ({upvote, downvote}) must be be supplied");
            }

            return Ok();
        }

        [HttpGet]
        [Route("{counter}")]
        public async Task<IHttpActionResult> Current(string counter)
        {
            try
            {
                var count = await _counter.GetCurrentCountAsync(counter).ContinueOnExecutionContext();
                return Ok(count);
            }
            catch (CounterNotFoundException)
            {
                return NotFound();
            }
        }

        private Func<string, Task> CounterAction(string action)
        {
            Func<string, Task> f;
            switch (action.ToUpperInvariant())
            {
                case "UPVOTE":
                    f = counter => _counter.IncrementAsync(counter, 1);
                    break;
                case "DOWNVOTE":
                    f = counter => _counter.DecrementAsync(counter, 1);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return f;
        }
    }
}