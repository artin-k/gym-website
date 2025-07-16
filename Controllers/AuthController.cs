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
                if (loginRequest.Username == "admin" && loginRequest.Password == "1234")
                {
                    return Ok(new { redirectUrl = "/admin-dashboard.html" });
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                return Ok(new { redirectUrl = "/profile-dashboard.html" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
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