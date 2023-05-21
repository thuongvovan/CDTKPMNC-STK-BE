using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _contentRootPath;

        public ImageController(IWebHostEnvironment environment)
        {
            _environment = environment;
            _contentRootPath = _environment.ContentRootPath;
        }

        // GET: <ImageController>
        [HttpGet("StoreBanner")]
        public PhysicalFileResult GetStoreBanner()
        {
            var folderPath = Path.Combine(_contentRootPath, "Images", "StoreBanners");
            string[] files = Directory.GetFiles(folderPath);
            var random = new Random();
            string randomFile = files[random.Next(files.Length)];
            return PhysicalFile(randomFile, "image/jpeg");
        }

        // GET: <ImageController>
        [HttpGet("ProductItem")]
        public PhysicalFileResult GetProductItemImage()
        {
            var folderPath = Path.Combine(_contentRootPath, "Images", "ProductItems");
            string[] files = Directory.GetFiles(folderPath);
            var random = new Random();
            string randomFile = files[random.Next(files.Length)];
            return PhysicalFile(randomFile, "image/jpeg");
        }
    }
}
