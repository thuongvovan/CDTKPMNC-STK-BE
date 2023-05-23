using Microsoft.AspNetCore.Mvc;
using System;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _webRootPath;

        public ImageController(IWebHostEnvironment environment)
        {
            _environment = environment;
            _webRootPath = _environment.WebRootPath;
        }

        // GET: <ImageController>/StoreBanner
        [HttpGet("StoreBanner")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public PhysicalFileResult GetStoreBanner()
        {
            var random = new Random();
            var folderPath = Path.Combine(_webRootPath, "DummyImages", "StoreBanners");
            string[] files = Directory.GetFiles(folderPath);
            string[] shuffledFiles = files.OrderBy(x => random.Next()).ToArray();
            string randomFile = shuffledFiles[random.Next(shuffledFiles.Length)];
            return PhysicalFile(randomFile, "image/jpeg");
        }

        // GET: <ImageController>/ProductItem
        [HttpGet("ProductItem")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public PhysicalFileResult GetProductItemImage()
        {
            var random = new Random();
            var folderPath = Path.Combine(_webRootPath, "DummyImages", "ProductItems");
            string[] files = Directory.GetFiles(folderPath);
            string[] shuffledFiles = files.OrderBy(x => random.Next()).ToArray();
            string randomFile = shuffledFiles[random.Next(shuffledFiles.Length)];
            return PhysicalFile(randomFile, "image/jpeg");
        }
    }
}
