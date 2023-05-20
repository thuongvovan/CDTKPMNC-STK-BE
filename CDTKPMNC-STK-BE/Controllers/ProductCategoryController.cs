using CDTKPMNC_STK_BE.Models;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductCategoryController : CommonController
    {
        private readonly ProductCategoryService _productCategoryService;
        public ProductCategoryController(ProductCategoryService productCategoryService) 
        {
            _productCategoryService = productCategoryService;
        }

        // POST /<ProductCategoryController>/Create
        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult AddProductCategory([FromBody] ProductCategoryRecord productCategoryRecord)
        {
            var validateSummary = _productCategoryService.ValidateProductCategoryRecord(productCategoryRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var isVerified = _productCategoryService.VerifyProductCategoryRecord(productCategoryRecord);
            if (isVerified)
            {
                var productCategory = _productCategoryService.CreateProductCategory(productCategoryRecord);
                return Ok(new ResponseMessage { Success = true, Message = "Create new product category successfuly.", Data = new { ProductCategory = productCategory } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Category is exist." });
        }

        // GET /<ProductCategoryController>/All
        [HttpGet("All")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetAllProductCategory()
        {
            var productCategories = _productCategoryService.GetAllProductCategory();
            if (productCategories.Count > 0)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of product categories successful.", Data = new { ProductCategories = productCategories } });
            }
            return Accepted(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { ProductCategories = productCategories } });
        }

        // GET /<ProductCategoryController>/Avalible
        [HttpGet("Avalible")]
        [Authorize(AuthenticationSchemes = "Account")]
        public IActionResult GetAvalibleProductCategory()
        {
            var productCategories = _productCategoryService.GetAvalibleProductCategory();
            if (productCategories.Count > 0)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of avalible product categories successful.", Data = new { ProductCategories = productCategories } });
            }
            return Accepted(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { ProductCategories = productCategories } });
        }

        // GET /<ProductCategoryController>/Disabled
        [HttpGet("Disabled")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetDisabledProductCategory()
        {
            var productCategories = _productCategoryService.GetDisabledProductCategory();
            if (productCategories.Count > 0)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of avalible product categories successful.", Data = new { ProductCategories = productCategories } });
            }
            return Accepted(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { ProductCategories = productCategories } });
        }

        // GET /<ProductCategoryController>/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpGet("{productCategoryId:Guid}")]
        [Authorize(AuthenticationSchemes = "Account")]
        public IActionResult GetProductCategory(Guid productCategoryId)
        {
            var productCategory = _productCategoryService.GetProductCategory(productCategoryId);
            if (productCategory != null)
            {
                if (productCategory.IsEnable)
                {
                    return Ok(new ResponseMessage { Success = true, Message = $"{productCategory.Id} is enable.", Data = new { ProductCategories = productCategory } });
                }
                else if (UserType == AccountType.Admin)
                {
                    return Ok(new ResponseMessage { Success = true, Message = $"{productCategory.Id} is disable.", Data = new { ProductCategories = productCategory } });
                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = $"{productCategoryId} is not valid." });
        }

        // DELETE /<ProductCategoryController>/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpDelete("{productCategoryId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult DeleteProductCategory(Guid productCategoryId)
        {
            var isSuccess = _productCategoryService.DeleteProductCategory(productCategoryId);
            if (isSuccess)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly deleted." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid delete request." });
        }

        // PUT /<ProductCategoryController>/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpPut("{productCategoryId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult UpdateProductCategory(Guid productCategoryId, [FromBody] ProductCategoryRecord productCategoryRecord)
        {
            var validateSummary = _productCategoryService.ValidateProductCategoryRecord(productCategoryRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var productCategory = _productCategoryService.UpdateProductCategory(productCategoryId, productCategoryRecord);
            if (productCategory != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly updated.", Data = new { ProductCategory = productCategory } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid update request." });
        }

        // PUT /<ProductCategoryController>/Disable/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpPut("Disable/{productCategoryId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult DisableProductCategory(Guid productCategoryId)
        {
            var productCategory = _productCategoryService.DisableProductCategory(productCategoryId);
            if (productCategory != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly updated.", Data = new { ProductCategory = productCategory } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid update request." });
        }

        // PUT /<ProductCategoryController>/Disable/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpPut("Enable/{productCategoryId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult EnableProductCategory(Guid productCategoryId)
        {
            var productCategory = _productCategoryService.EnableProductCategory(productCategoryId);
            if (productCategory != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly updated.", Data = new { ProductCategory = productCategory } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid update request." });
        }
    }
}
