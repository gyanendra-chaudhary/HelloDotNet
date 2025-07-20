using Microsoft.AspNetCore.Mvc;

namespace NLogDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("all-logs")]
        public IActionResult LogAllLevels()
        {
            _logger.LogTrace("LogTrace: Entering the LogAllLevels endpoint with Trace-level logging.");

            // Simulate a variable and log it at Trace level
            int calculation = 5 * 10;
            _logger.LogTrace("LogTrace: Calculation value is {calculation}", calculation);

            _logger.LogDebug("LogDebug: Initializing debug-level logs for debugging purposes.");

            // Log some debug information
            var debugInfo = new { Action = "LogAllLevels", Status = "Debugging" };
            _logger.LogDebug("LogDebug: Debug information: {@debugInfo}", debugInfo);

            _logger.LogInformation("LogInformation: The LogAllLevels endpoint was reached successfully.");

            // Simulate a condition that might cause an issue
            bool resourceLimitApproaching = true;
            if (resourceLimitApproaching)
            {
                _logger.LogWarning("LogWarning: Resource usage is nearing the limit. Action may be required soon.");
            }

            try
            {
                // Simulate an error scenario
                int x = 0;
                int result = 10 / x;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }

            // Log a critical error scenario
            bool criticalFailure = true;
            if (criticalFailure)
            {
                _logger.LogCritical(
                    "LogCritical: A critical system failure has been detected. Immediate attention is required.");
            }

            return Ok("All logging levels demonstrated in this endpoint.");
        }

        [HttpGet("log-with-new-values")]
        public IActionResult WithNewValues()
        {
            var UniqueId = Guid.NewGuid();
            try
            {
                _logger.LogTrace("{UniqueId} This is a Trace log, the most detailed information.", UniqueId);
                _logger.LogDebug("{UniqueId} This is a Debug log, useful for debugging.", UniqueId);
                _logger.LogInformation("{UniqueId} This is an Information log, general info about app flow.", UniqueId);
                _logger.LogWarning("This is a Warning log, indicating a potential issue.{UniqueId}", UniqueId);
                _logger.LogCritical("This is a Critical log, indicating a serious failure in the application.");
                //Simulating an error situation
                int x = 10, y = 0;
                int z = x / y;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{UniqueId} This is an Error log, indicating a failure in the current operation.", UniqueId);
            }
            return Ok("Check your logs to see the different logging levels in action!");
        }
    }
}