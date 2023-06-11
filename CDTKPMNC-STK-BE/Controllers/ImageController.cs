using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : CommonController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _webRootPath;
        private readonly string _uploadDirectory = Environment.GetEnvironmentVariable("UPLOAD_DIRECTORY")!;
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png"};
        private readonly string _uploadRequestPath = Environment.GetEnvironmentVariable("UPLOAD_REQUEST_PATH")!;

        public ImageController(IWebHostEnvironment environment)
        {
            _environment = environment;
            _webRootPath = _environment.WebRootPath;
        }

        //// GET: <ImageController>/StoreBanner
        //[HttpGet("StoreBanner")]
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public PhysicalFileResult GetStoreBanner()
        //{
        //    var random = new Random();
        //    var folderPath = Path.Combine(_webRootPath, "DummyImages", "StoreBanners");
        //    string[] files = Directory.GetFiles(folderPath);
        //    string[] shuffledFiles = files.OrderBy(x => random.Next()).ToArray();
        //    string randomFile = shuffledFiles[random.Next(shuffledFiles.Length)];
        //    return PhysicalFile(randomFile, "image/jpeg");
        //}

        //// GET: <ImageController>/StoreBanner/Name
        //[HttpGet("StoreBanner/Name")]
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult GetStoreBannerName()
        //{
        //    var random = new Random();
        //    var folderPath = Path.Combine(_webRootPath, "DummyImages", "StoreBanners");
        //    string[] files = Directory.GetFiles(folderPath);
        //    string[] shuffledFiles = files.OrderBy(x => random.Next()).ToArray();
        //    string randomFile = shuffledFiles[random.Next(shuffledFiles.Length)];
        //    string fileRoute = randomFile.Replace(_webRootPath, "");
        //    return Ok(new ResponseMessage(true, "Random store banner path", new { BannerPath = fileRoute }));
        //}

        //// GET: <ImageController>/ProductItem
        //[HttpGet("ProductItem")]
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public PhysicalFileResult GetProductItemImage()
        //{
        //    var random = new Random();
        //    var folderPath = Path.Combine(_webRootPath, "DummyImages", "ProductItems");
        //    string[] files = Directory.GetFiles(folderPath);
        //    string[] shuffledFiles = files.OrderBy(x => random.Next()).ToArray();
        //    string randomFile = shuffledFiles[random.Next(shuffledFiles.Length)];
        //    return PhysicalFile(randomFile, "image/jpeg");
        //}

        //// GET: <ImageController>/ProductItem/Name
        //[HttpGet("ProductItem/Name")]
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult GetProductItemImageName()
        //{
        //    var random = new Random();
        //    var folderPath = Path.Combine(_webRootPath, "DummyImages", "ProductItems");
        //    string[] files = Directory.GetFiles(folderPath);
        //    string[] shuffledFiles = files.OrderBy(x => random.Next()).ToArray();
        //    string randomFile = shuffledFiles[random.Next(shuffledFiles.Length)];
        //    string fileRoute = randomFile.Replace(_webRootPath, "");
        //    return Ok(new ResponseMessage(true, "Random product item image path", new { ImagePath = fileRoute }));
        //}

        // GET: <ImageController>/UploadImage
        [HttpPost("Upload")]
        [Authorize(AuthenticationSchemes = "Account")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new ResponseMessage(false, "No files to upload"));

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedImageExtensions.Contains(extension))
                return BadRequest(new ResponseMessage(false, "Allowed extensions are jpg, jpeg, png"));

            if (file.Length > 600 * 1024)
                return BadRequest(new ResponseMessage(false, "Not allowed files larger than 600KB"));
            string newFilename = UserId.ToString() + "_" + file.FileName;
            string filePath = Path.Combine(_uploadDirectory, "TempImages", newFilename);
            string directoryPath = Path.GetDirectoryName(filePath)!;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            string fileRoute = $"{_uploadRequestPath}/TempImages/{newFilename}";
            return Ok(new ResponseMessage(true, "Upload successfully", new { ImagePath = fileRoute}));
        }

    }
}
