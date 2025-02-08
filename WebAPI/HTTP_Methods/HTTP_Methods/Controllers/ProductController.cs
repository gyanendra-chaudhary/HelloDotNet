using HTTP_Methods.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTTP_Methods.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController:ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public ProductController(AppDBContext dBContext) => _dbContext = dBContext;

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _dbContext.Products.ToListAsync();
                return Ok(products);    
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = "An error occured while retriving products", Details = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _dbContext.Products.FindAsync(id);
                if (product == null)
                    return NotFound(new { Message = $"Product with ID {id} not found." });
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occured while retriving the product.", Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();    
                return CreatedAtAction(nameof(GetProductById), new {id=product.Id}, product);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = "An error occured while creating the product.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            try
            {
                if(id != product.Id)
                    return BadRequest("Product ID mismatch.");
                var existingProduct = await _dbContext.Products.FindAsync(id);
                if(existingProduct == null)
                    return NotFound(new { Message = $"Product with ID {id} not found." });
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price; 

                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the product.", Details = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occured while updating the product.", Details = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProductPrice(int id, Product product)
        {
            try
            {
                if(id != product.Id)
                    return BadRequest("Product ID mismatch.");
                var existingProduct = await _dbContext.Products.FindAsync(id);
                if(existingProduct == null)
                    return NotFound(new { Message = $"Product with ID {id} not found." });
                existingProduct.Price = product.Price;
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occured while updating the product.", Details = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _dbContext.Products.FindAsync(id);
                if(product == null)
                    return NotFound(new {Message = $"Product with ID {id} not found."});
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the product.", Details = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = "An error occured while deleting the product", Details = ex.Message });
            }
        }
        [HttpHead("{id}")]
        public async Task<IActionResult> HeadProduct(int id)
        {
            try
            {
                var productExists = await _dbContext.Products.AnyAsync(p => p.Id == id);
                if (!productExists)
                    return NotFound();
                Response.Headers.Append("Content-Type", "application/json");

                var contentLength = System.Text.Json.JsonSerializer.Serialize(productExists).Length;
                Response.Headers.Append("Content-Length", contentLength.ToString());

                Response.Headers.Append("X-Custom-Header", "This is a custom header");
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occured while retriving product meta data.", Details = ex.Message });
            }
        }

        [HttpOptions]
        public async Task<IActionResult> GetOptions()
        {
            try
            {
                Response.Headers.Append("Allow", "GET, POST, PUT, PATCH, DELETE, OPTIONS, HEAD");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving options.", Details = ex.Message });
            }
        }
    }
}
