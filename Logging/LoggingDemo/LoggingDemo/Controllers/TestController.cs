using Microsoft.AspNetCore.Mvc;

namespace LoggingDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController:ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
            _logger.LogInformation("Test controller started");
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation($"Executing TestController.Get Method");
            return Ok();
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

            bool resourceLimitApproaching = true;
            if (resourceLimitApproaching)
            {
                _logger.LogWarning("LogWarning: Resource usage is nearing the limit. Action may be required soon.");
            }
            try
            {
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
                _logger.LogCritical("LogCritical: A critical system failure has been detected. Immediate attention is required.");
            }
            return Ok("All logging levels demonstrated in this endpoint.");
        }
    }
}
