using Microsoft.AspNetCore.Mvc;
using gymWebsite.Models;
using static System.Net.Mime.MediaTypeNames;

namespace gymWebsite.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly MyDbContext _context;

        public UploadController(IWebHostEnvironment env, MyDbContext context)
        {
            _env = env;
            _context = context;
        }

        [HttpPost]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> UploadImages(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded.");

            var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var result = new List<object>();

            foreach (var file in files)
            {
                var newFileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);
                var savePath = Path.Combine(uploadPath, newFileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = $"/uploads/{newFileName}";

                // ✅ Save to database
                var image = new ImageModel
                {
                    FileName = file.FileName,
                    FilePath = relativePath
                };
                _context.Images.Add(image);

                result.Add(new
                {
                    FileName = file.FileName,
                    FilePath = relativePath
                });
            }

            await _context.SaveChangesAsync();

            return Ok(result);
        }
    }
}
