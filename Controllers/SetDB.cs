using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Threading.Tasks;

namespace gymWebsite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetDB : ControllerBase
    {
        private readonly string _connectionString = "Server=localhost;User=root;Password=your_root_password;";

        [HttpPost("setup")]
        public async Task<IActionResult> SetupDatabase()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    Console.WriteLine("اتصال به سرور MySQL با موفقیت برقرار شد.");

                    // ایجاد پایگاه داده
                    string createDatabaseQuery = "CREATE DATABASE IF NOT EXISTS gymDB;";
                    using (MySqlCommand command = new MySqlCommand(createDatabaseQuery, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("پایگاه داده ایجاد شد.");
                    }

                    // تغییر رشته اتصال برای استفاده از پایگاه داده جدید
                    string newConnectionString = "Server=localhost;Database=gymDB;User=root;Password=your_password;";
                    connection.ConnectionString = newConnectionString;

                    // ایجاد جدول
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS users (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            name VARCHAR(100),
                            email VARCHAR(100)
                        );
                    ";
                    using (MySqlCommand command = new MySqlCommand(createTableQuery, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("جدول users ایجاد شد.");
                    }

                    // درج داده در جدول
                    string insertQuery = "INSERT INTO users (name, email) VALUES ('علی', 'ali@example.com');";
                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("داده در جدول درج شد.");
                    }

                    // خواندن داده از جدول
                    string selectQuery = "SELECT * FROM users;";
                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Console.WriteLine($"ID: {reader["id"]}, Name: {reader["name"]}, Email: {reader["email"]}");
                            }
                        }
                    }
                }

                return Ok("پایگاه داده و جدول با موفقیت تنظیم شدند.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"خطا در تنظیم پایگاه داده: {ex.Message}");
            }
        }
    }
}
