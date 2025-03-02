using Microsoft.AspNetCore.Mvc;
using NLogDemo.Models;

namespace NLogDemo.Controllers;
[ApiController]
[Route("/api/[controller]")]
public class BooksController:ControllerBase
{
    private readonly ILogger<BooksController> _logger;

    private static readonly List<Book> Books = new()
    {
        new Book() { Id = 1001, Title = "ASP.NET Core", Author = "Gyanendra", YearPublished = 2019 },
        new Book() { Id = 1002, Title = "SQL Server", Author = "Gyanendra", YearPublished = 2022 }
    };
    public BooksController(ILogger<BooksController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Post(Book book)
    {
        Books.Add(book);
        _logger.LogInformation("Added a new book {@Book}", book);
        return Ok();
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        _logger.LogInformation("Retrieved {@Books} books", Books);
        return Ok(Books);
    }
}