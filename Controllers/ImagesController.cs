using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gymWebsite.Models;

namespace gymWebsite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ImagesController(MyDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("homepage")]
        public async Task<IActionResult> GetHomepageImages()
        {
            var images = await _context.Images
                .OrderBy(i => i.Id)
                .Take(3)
                .Select(i => new
                {
                    i.Id,
                    i.FileName,
                    FilePath = i.FilePath
                })
                .ToListAsync();

            return Ok(images);
        }

    }
}


/*            // Optional file existence check — only helpful during development
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
            foreach (var image in images)
            {
                var fullPath = Path.Combine(uploadPath, image.FileName);
                if (!System.IO.File.Exists(fullPath))
                {
                    Console.WriteLine($"[⚠] Missing file: {fullPath}");
                }
            }*/