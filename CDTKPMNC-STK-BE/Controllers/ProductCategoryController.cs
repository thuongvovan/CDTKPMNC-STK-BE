using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Repositories;
using CDTKPMNC_STK_BE.Utilities.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using CDTKPMNC_STK_BE.Utilities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : AppBaseController
    {
        public ProductCategoryController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        // POST /<UserController>/ProductCategory
        [HttpPost("ProductCategory")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult AddProductCategory([FromBody] ProductCategoryInfo categoryInfo)
        {
            var validator = new ProductCategoryValidator();
            ValidationResult? validateResult;
            try
            {
                validateResult = validator.Validate(categoryInfo);
            }
            catch (Exception)
            {

                return BadRequest(new ResponseMessage { Success = false, Message = "Unable to verify data" });
            }

            if (!validateResult.IsValid)
            {
                string? ErrorMessage = validateResult.Errors?.FirstOrDefault()?.ErrorMessage;
                return BadRequest(new ResponseMessage { Success = false, Message = ErrorMessage! });
            }
            var curentCategory = _unitOfWork.ProductCategoryRepo.GetByName(categoryInfo.Name);
            if (curentCategory == null)
            {
                _unitOfWork.ProductCategoryRepo.Add(categoryInfo);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Create new game successfuly." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Category is exist." });
        }

        // GET /<UserController>/ProductCategory/All
        [HttpGet("ProductCategory/All")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetAllProductCategory()
        {
            var productCategories = _unitOfWork.ProductCategoryRepo.GetAll();
            if (productCategories != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of product categories successful.", Data = new { ProductCategories = productCategories } });
            }
            productCategories = new List<ProductCategory>();
            return Accepted(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { ProductCategories = productCategories } });
        }

        // GET /<UserController>/ProductCategory/Avalible
        [HttpGet("ProductCategory/Avalible")]
        [Authorize(AuthenticationSchemes = "Account")]
        public IActionResult GetAvalibleProductCategory()
        {
            var productCategories = _unitOfWork.ProductCategoryRepo.GetAvalible();
            if (productCategories != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of avalible product categories successful.", Data = new { ProductCategories = productCategories } });
            }
            productCategories = new List<ProductCategory>();
            return Accepted(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { ProductCategories = productCategories } });
        }

        // GET /<UserController>/ProductCategory/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpGet("ProductCategory/{productCategoryId:Guid}")]
        [Authorize(AuthenticationSchemes = "Account")]
        public IActionResult GetProductCategory(Guid productCategoryId)
        {
            var productCategory = _unitOfWork.ProductCategoryRepo.GetById(productCategoryId);
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

        // DELETE /<UserController>/ProductCategory/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpDelete("ProductCategory/{productCategoryId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult DeleteProductCategory(Guid productCategoryId)
        {
            var productCategory = _unitOfWork.ProductCategoryRepo.GetById(productCategoryId);
            if (productCategory != null && (productCategory.Items == null || productCategory.Items.Count == 0))
            {
                _unitOfWork.ProductCategoryRepo.Delete(productCategory);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly deleted." });
            }
            return Ok(new ResponseMessage { Success = false, Message = "Invalid delete request." });
        }

        // PUT /<UserController>/ProductCategory/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpPut("ProductCategory/{productCategoryId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult UpdateProductCategory([FromBody] ProductCategoryInfo categoryInfo, Guid productCategoryId)
        {
            var productCategory = _unitOfWork.ProductCategoryRepo.GetById(productCategoryId);
            if (productCategory != null)
            {
                var validator = new ProductCategoryValidator();
                ValidationResult validateResult = validator.Validate(categoryInfo);
                if (validateResult.IsValid)
                {
                    _unitOfWork.ProductCategoryRepo.Update(productCategory, categoryInfo);
                    _unitOfWork.Commit();
                    return Ok(new ResponseMessage { Success = true, Message = "Successfuly updated.", Data = new { ProductCategory = productCategory } });
                }
                string? ErrorMessage = validateResult.Errors?.FirstOrDefault()?.ErrorMessage;
                return BadRequest(new ResponseMessage { Success = false, Message = ErrorMessage });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid update request." });
        }

        // PUT /<UserController>/ProductCategory/Disable/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpPut("ProductCategory/Disable/{productCategoryId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult DisableProductCategory(Guid productCategoryId)
        {
            var productCategory = _unitOfWork.ProductCategoryRepo.GetById(productCategoryId);
            if (productCategory != null)
            {
                _unitOfWork.ProductCategoryRepo.Disable(productCategory);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly updated.", Data = new { ProductCategory = productCategory } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid update request." });
        }

        // PUT /<UserController>/ProductCategory/Disable/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpPut("ProductCategory/Enable/{productCategoryId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult EnableProductCategory(Guid productCategoryId)
        {
            var productCategory = _unitOfWork.ProductCategoryRepo.GetById(productCategoryId);
            if (productCategory != null)
            {
                _unitOfWork.ProductCategoryRepo.Enable(productCategory);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly updated.", Data = new { ProductCategory = productCategory } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid update request." });
        }
    }
}
