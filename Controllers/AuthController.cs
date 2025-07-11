using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using System;
using gymWebsite.Models;

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
        public async Task<IActionResult> Register([FromBody] UserClass user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                    return BadRequest("Username and password are required");

                if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                    return BadRequest("Username already exists");

                var newUser = new User
                {
                    Username = user.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password)
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserClass loginRequest)
        {
            try
            {
                // Hardcoded admin check
                if (loginRequest.Username == "admin" && loginRequest.Password == "1234")
                {
                    var admin = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
                    if (admin == null)
                    {
                        admin = new User
                        {
                            Username = "admin",
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234")
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
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
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

        [HttpPost("admin-dashboard")]
        public async Task<IActionResult> Management()
        {
            try
            {
                // بررسی اینکه کاربر وارد شده ادمین است
                if (!User.IsInRole("admin"))
                {
                    return Unauthorized("Access denied. Only admins can access this resource.");
                }

                // بازیابی لیست کاربران از پایگاه داده
                var users = await _context.Users.ToListAsync();

                // بازگشت لیست کاربران به عنوان پاسخ JSON
                return Ok(users);
            }
            catch (System.Exception ex)
            {
                // بازگشت خطا در صورت بروز مشکل
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}