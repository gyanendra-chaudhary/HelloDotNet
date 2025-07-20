using LoggingDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoggingDemo.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SerilLogController : Controller
{
    private readonly ILogger<SerilLogController> _logger;

    private static List<Book> _books = new List<Book>()
    {
        new Book() { Id = 1001, Title = "ASP.NET Core", Author = "Gyanendra Chaudhary", YearPublished = 2019 },
        new Book() { Id = 1002, Title = "SQL Server", Author = "Gyanendra Chaudhary", YearPublished = 2019 }
    };
    
    
    public SerilLogController(ILogger<SerilLogController> logger)
    {
        _logger = logger;   
    }
    [HttpPost]
    public IActionResult Post([FromBody] Book book)
    {
        _books.Add(book);
        _logger.LogInformation("Added a new book {@Book}", book);
        return Ok("Success");
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        string UniqueId = Guid.NewGuid().ToString();
        _logger.LogInformation("{UniqueId} This is an Information log, general info about app flow.", UniqueId);
        _logger.LogInformation($"Getting books from {@_books}", _books);
        return Ok(_books);
    }
    
    [HttpGet("all-logs")]
    public IActionResult LogAllLevels()
    {
        _logger.LogTrace("LogTrace: Entering the LogAllLevels endpoint with Trace-level logging.");
        int calculation = 5 * 10;
        _logger.LogTrace("LogTrace: Calculation value is {calculation}", calculation);
        _logger.LogDebug("LogDebug: Initializing debug-level logs for debugging purpose.");
        
        // Log some debug information
        var debugInfo = new {Action = "LogAllLevels", Status="Debugging"};
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
            _logger.LogCritical("LogCritical: A critical system failure has been detected. Immediate attention is required.");
        }
        return Ok("All logging levels demonstrated in this endpoint.");
    }
}