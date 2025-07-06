using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using gymWebsite;       // For your User model
using gymWebsite.Data;  // For your MyDbContext
using System;

namespace gymWebsite.Controllers
{
    [ApiController]
    [Route("/api/users/")]
    public class AuthController : ControllerBase
    {
        private readonly MyDbContext _context;

        public AuthController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user )
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                {
                    return BadRequest("Username and password are required");
                }

                // Check if user exists
                if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                {
                    return BadRequest("Username already exists");
                }

                // Hash password
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                // Add to database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginRequest)
        {
            try
            {
                // Hardcoded admin check
                if (loginRequest.Username == "admin" && loginRequest.Password == "1234")
                {
                    // Check if admin exists in DB, if not create one
                    var admin = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
                    if (admin == null)
                    {
                        admin = new User
                        {
                            Username = "admin",
                            Password = BCrypt.Net.BCrypt.HashPassword("1234")
                        };
                        _context.Users.Add(admin);
                        await _context.SaveChangesAsync();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Role, "admin")
                    };

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                        authProperties);

                    return Ok(new { RedirectUrl = "/admin-dashboard" });
                }

                // Normal user login
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
                {
                    return Unauthorized("Invalid username or password");
                }

                var userClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "user") // Default role
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme)),
                    new AuthenticationProperties { IsPersistent = true });

                return Ok(new { RedirectUrl = "/dashboard" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Logout successful" });
        }
    }
}