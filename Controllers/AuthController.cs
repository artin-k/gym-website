using Microsoft.AspNetCore.Mvc;
using MyBackend.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BCrypt.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MyBackend.Controllers
{
    [ApiController]
    [Route("/api/users/")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDto request)
        {
            try
            {
                var tables = await _context.Database.ExecuteSqlRawAsync("SELECT name FROM sqlite_master WHERE type='table'");
                Console.WriteLine($"Tables found: {tables}");

                if (await _context.users.AnyAsync(u => u.Username == request.Username))
                    return BadRequest("Username already exists");

                var user = new User
                {
                    Username = request.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
                };

                _context.users.Add(user);
                await _context.SaveChangesAsync();

                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Login user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Invalid request data");

            var user = await _context.users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return Unauthorized("User not found");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return Unauthorized("Invalid password");

            // Create user claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Keep login active even after browser close
                ExpiresUtc = DateTime.UtcNow.AddDays(7) // Cookie expires in 7 days
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          new ClaimsPrincipal(claimsIdentity), authProperties);

            return Ok(new { message = "Login successful", username = user.Username });
        }


        // Logout user
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logged out successfully");
        }
    }

    // DTO for user registration
    public class UserRegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // DTO for user login
    public class UserLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
