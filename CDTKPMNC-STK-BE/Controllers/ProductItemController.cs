using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductItemController : CommonController
    {
        private readonly ProductItemService _productItemService;
        private readonly StoreService _storeService;
        public Guid? StoreId 
        { 
            get
            {
                if (UserType == Models.AccountType.Partner)
                {
                    return UserId;
                }
                return null;
            } 
        }
         public  ProductItemController(ProductItemService productItemService, StoreService storeService)
        {
            _productItemService = productItemService;
            _storeService = storeService;
        }
    
        // POST: /<ProductItemController>/Create
        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult AddProductItem([FromBody] ProductItemRecord productItemRecord)
        {
            var validateSummary = _productItemService.ValidateProductItemRecord(productItemRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var isVerified = _productItemService.VerifyCreateProductItem(productItemRecord, StoreId!.Value);
            if (isVerified)
            {
                var productItem = _productItemService.CreateProductItem(productItemRecord, UserId);
                return Ok(new ResponseMessage { Success = true, Message = "Create new product item successfuly.", Data = new { ProductItem = productItem } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Product Item is existed." });
        }

        // PUT: /<ProductItemController>/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpPut("{productItemId:Guid}")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult UpdateProductItem(Guid productItemId, [FromBody] ProductItemRecord productItemRecord)
        {
            var validateSummary = _productItemService.ValidateProductItemRecord(productItemRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var productItem = _productItemService.GetById(productItemId);
            if (productItem != null)
            {
                var isVerified = _productItemService.VerifyUpdateProductItem(productItem, UserId, UserType);
                if (isVerified)
                {
                    productItem = _productItemService.UpdateProductItem(productItem!, productItemRecord);
                    return Ok(new ResponseMessage { Success = true, Message = "Update product item successfuly.", Data = new { ProductItem = productItem } });
                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Product Item does not existed." });
        }

        // PUT: /<ProductItemController>/Disable/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpPut("Disable/{productItemId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult DisableProductItem(Guid productItemId)
        {
            var productItem = _productItemService.GetById(productItemId);
            if (productItem != null)
            {
                var isVerified = _productItemService.VerifyUpdateProductItem(productItem, UserId, UserType);
                if (isVerified)
                {
                    productItem = _productItemService.DisableProductItem(productItem!);
                    return Ok(new ResponseMessage { Success = true, Message = "Disable product item successfuly.", Data = new { ProductItem = productItem } });
                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Product Item does not existed." });
        }

        // PUT: /<ProductItemController>/Enable/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpPut("Enable/{productItemId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult EnableProductItem(Guid productItemId)
        {
            var productItem = _productItemService.GetById(productItemId);
            if (productItem != null)
            {
                var isVerified = _productItemService.VerifyUpdateProductItem(productItem, UserId, UserType);
                if (isVerified)
                {
                    productItem = _productItemService.EnableProductItem(productItem!);
                    return Ok(new ResponseMessage { Success = true, Message = "Enable product item successfuly.", Data = new { ProductItem = productItem } });
                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Product Item does not existed." });
        }

        // DELETE: /<ProductItemController>/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpDelete("{productItemId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult DeleteProductItem(Guid productItemId)
        {
            var productItem = _productItemService.GetById(productItemId);
            if (productItem != null)
            {
                var isVerified = _productItemService.VerifyDeleteProductItem(productItem, UserId, UserType);
                if (isVerified)
                {
                    _productItemService.DeleteProductItem(productItem!);
                    return Ok(new ResponseMessage { Success = true, Message = "Delete product item successfuly." });
                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Product Item does not existed." });
        }

        // GET: /<ProductItemController>/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpGet("{productItemId:Guid}")]
        [Authorize(AuthenticationSchemes = "Account")]
        public IActionResult GetProductItem(Guid productItemId)
        {
            var productItem = _productItemService.GetById(productItemId);
            if (productItem != null)
            {
                var isVerified = _productItemService.VerifyGetProductItem(productItem, UserId, UserType);
                if (isVerified)
                {
                    return Ok(new ResponseMessage { Success = true, Message = "Get product item successfuly.", Data = new { ProductItem = productItem } });
                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Product Item does not existed." });
        }

        // GET: /<ProductItemController>/All
        [HttpGet("All")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetAllProductItem()
        {
            var productItems = _productItemService.GetAllByAccountType(UserId, UserType);
            if (productItems.Count > 0)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get all product item successfuly.", Data = new { ProductItems = productItems } });
            }
            return Ok(new ResponseMessage { Success = true, Message = "Product item list is empty.", Data = new { ProductItems = productItems } });
        }

        // GET: /<ProductItemController>/All/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpGet("All/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult GetAllByStore(Guid storeId)
        {
            var store = _storeService.GetById(storeId);
            if (store != null)
            {
                var productItems = _productItemService.GetAllByStoreByAccountType(store, UserId, UserType);
                if (productItems != null)
                {
                    if (productItems.Count > 0)
                    {
                        return Ok(new ResponseMessage { Success = true, Message = "Get all product item by store successfuly.", Data = new { ProductItems = productItems } });
                    }
                    return Ok(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { ProductItems = productItems } });

                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Store does not exist." });
        }

        // GET: /<ProductItemController>/Available
        [HttpGet("Available")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult GetAvailableProductItem()
        {
            var productItems = _productItemService.GetAvailableByAccountType(UserId, UserType);
            if (productItems.Count > 0)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get all available product item successfuly.", Data = new { ProductItems = productItems } });
            }
            return Ok(new ResponseMessage { Success = true, Message = "Product item list is empty.", Data = new { ProductItems = productItems } });
        }

        // GET: /<ProductItemController>/Available/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpGet("Available/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Account")]
        public IActionResult GetAvailableByStore(Guid storeId)
        {
            var store = _storeService.GetById(storeId);
            if (store != null)
            {
                var productItems = _productItemService.GetAvailableByStoreByAccountType(store, UserId, UserType);
                if (productItems != null)
                {
                    if (productItems.Count > 0)
                    {
                        return Ok(new ResponseMessage { Success = true, Message = "Get available product item by store successfuly.", Data = new { ProductItems = productItems } });
                    }
                    return Ok(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { ProductItems = productItems } });

                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Store does not exist." });
        }

        // GET: /<ProductItemController>/Disabled
        [HttpGet("Disabled")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult GetDisabledProductItem()
        {
            var productItems = _productItemService.GetDisabledByAccountType(UserId, UserType);
            if (productItems.Count > 0)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get all available product item successfuly.", Data = new { ProductItems = productItems } });
            }
            return Ok(new ResponseMessage { Success = true, Message = "Product item list is empty.", Data = new { ProductItems = productItems } });
        }

        // GET: /<ProductItemController>/Disabled/D9D05EAD-165D-45A9-2FE8-08DB52449ED0
        [HttpGet("Disabled/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult GetDisabledByStore(Guid storeId)
        {
            var store = _storeService.GetById(storeId);
            if (store != null)
            {
                var productItems = _productItemService.GetDisabledByStoreByAccountType(store, UserId, UserType);
                if (productItems != null)
                {
                    if (productItems.Count > 0)
                    {
                        return Ok(new ResponseMessage { Success = true, Message = "Get disabled product item by store successfuly.", Data = new { ProductItems = productItems } });
                    }
                    return Ok(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { ProductItems = productItems } });

                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Store does not exist." });
        }
    }
}
