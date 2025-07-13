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
                .OrderBy(i => i.SortOrder)
                .Take(3)
                .Select(i => new
                {
                    i.Id,
                    i.FileName,
                    i.FilePath
                })
                .ToListAsync();

            return Ok(images);
        }

        // GET: api/images/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _context.Images
                .OrderBy(i => i.SortOrder)
                .ToListAsync();

            return Ok(images);
        }

        // DELETE: api/images/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
                return NotFound();

            var fullPath = Path.Combine(_env.WebRootPath, image.FilePath.TrimStart('/'));

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/images/sort/{id}
        [HttpPut("sort/{id}")]
        public async Task<IActionResult> UpdateSort(int id, [FromBody] int newSort)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return NotFound();

            image.SortOrder = newSort;
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
