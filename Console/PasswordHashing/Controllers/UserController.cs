using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordHashing.Models;
using PasswordHashing.Services;

namespace PasswordHashing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _dbContext;
        public UserController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<User>> Register(UserDTO userDTO)
        {
            try
            {
                byte[] passwordHash, passwordSalt;
                HashingService.CreatePasswordHash(userDTO.Password, out passwordHash, out passwordSalt);
                User user = new User
                {
                    Username = userDTO.UserName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(UserDTO userDTO)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == userDTO.UserName);
                if (user == null || !HashingService.VerifyPasswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return Unauthorized("Invalid username or password");
                }
                return Ok($"User {userDTO.UserName} logged in successfully");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
