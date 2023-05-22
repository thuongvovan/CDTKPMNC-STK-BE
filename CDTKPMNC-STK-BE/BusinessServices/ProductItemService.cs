using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.RecordValidators;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class ProductItemService : CommonService
    {
        private readonly ProductCategoryService _categoryService;
        private readonly IProductItemRepository _productItemRepo;
        public ProductItemService(IUnitOfWork unitOfWork, ProductCategoryService categoryService) : base(unitOfWork) 
        {
            _categoryService = categoryService;
            _productItemRepo = _unitOfWork.ProductItemRepo;
        }

        public ProductItem? GetById(Guid productItem)
        {
            return _productItemRepo.GetById(productItem);
        }

        public List<ProductItem> GetAll()
        {
            return _productItemRepo.GetAll().ToList();
        }

        public List<ProductItem> GetAllByAccountType(Guid UserId, AccountType UserType)
        {
            if (UserType == AccountType.Admin)
            {
                return _productItemRepo.GetAll().ToList();
            }
            else if (UserType == AccountType.Partner)
            {
                return _productItemRepo.GetAllByStore(UserId).ToList();
            }
            return new List<ProductItem>(0);
        }

        public List<ProductItem>? GetAllByStoreByAccountType(Store store, Guid UserId, AccountType UserType)
        {
            if (UserType == AccountType.Admin || (UserType == AccountType.Partner && store.Id == UserId))
            {
                if (store.ProductItems != null)
                {
                    return store.ProductItems!.ToList();
                }
                return new List<ProductItem>(0);
            }
            return null;
        }

        public List<ProductItem>? GetAvailableByStoreByAccountType(Store store, Guid UserId, AccountType UserType)
        {
            if (UserType == AccountType.Admin || (UserType == AccountType.Partner && store.Id == UserId))
            {
                if (store.ProductItems != null)
                {
                    return store.ProductItems!.Where(i => i.IsEnable).ToList();
                }
                return new List<ProductItem>(0);
            }
            if (store.IsEnable)
            {
                if (store.ProductItems != null)
                {
                    return store.ProductItems!.Where(i => i.IsEnable).ToList();
                }
                return new List<ProductItem>(0);
            }
            return null;
        }

        public List<ProductItem> GetAvailableByAccountType(Guid UserId, AccountType UserType)
        {
            if (UserType == AccountType.Admin)
            {
                return _productItemRepo.GetAvalible();
            }
            else if (UserType == AccountType.Partner)
            {
                return _productItemRepo.GetAvalibleByStore(UserId).ToList();
            }
            return new List<ProductItem>(0);
        }

        public List<ProductItem> GetDisabledByAccountType(Guid UserId, AccountType UserType)
        {
            if (UserType == AccountType.Admin)
            {
                return _productItemRepo.GetDisabled();
            }
            else if (UserType == AccountType.Partner)
            {
                return _productItemRepo.GetDisabledByStore(UserId).ToList();
            }
            return new List<ProductItem>(0);
        }

        public List<ProductItem>? GetDisabledByStoreByAccountType(Store store, Guid UserId, AccountType UserType)
        {
            if (UserType == AccountType.Admin || (UserType == AccountType.Partner && store.Id == UserId))
            {
                if (store.ProductItems != null)
                {
                    return store.ProductItems!.Where(i => !i.IsEnable).ToList();
                }
                return new List<ProductItem>(0);
            }
            return null;
        }

        public ValidationSummary ValidateProductItemRecord(ProductItemRecord? productItemRecord)
        {
            if (productItemRecord == null)
            {
                return new ValidationSummary(false, "Product item info is required.");
            }
            var validator = new ProductItemRecordValidator(_categoryService);
            var result = validator.Validate(productItemRecord);
            return result.GetSummary();
        }

        public bool VerifyCreateProductItem(ProductItemRecord productItemRecord, Guid storeId)
        {
            var productItem = _productItemRepo.GetByNameByStore(productItemRecord.Name!, storeId);
            if (productItem == null) return true;
            return false;
        }

        public bool VerifyUpdateProductItem(ProductItem productItem, Guid userId, AccountType accountType)
        {
            if (accountType == AccountType.Admin) return true;
            if (accountType == AccountType.Partner && productItem.StoreId == userId) return true;
            return false;
        }

        public bool VerifyDeleteProductItem(ProductItem productItem, Guid userId, AccountType userType)
        {
            // Kiểm tra điều kiện đã đạt hàng hay chưa, để sau vì chưa có chức năng mua hàng
            if (userType == AccountType.Admin)
            {
                return true;
            }
            else if(userType == AccountType.Partner && productItem.StoreId == userId)
            {
                return true;
            }    
            return false;
        }

        public bool VerifyGetProductItem(ProductItem productItem, Guid userId, AccountType userType)
        {
            // Kiểm tra điều kiện đã đạt hàng hay chưa, để sau.
            if (userType == AccountType.Admin)
            {
                return true;
            }
            else if (userType == AccountType.Partner && productItem.StoreId == userId)
            {
                return true;
            }
            if (productItem.IsEnable && productItem.Store.IsEnable)
            {
                return true;
            }
            return false;
        }

        public ProductItem CreateProductItem(ProductItemRecord productItemRecord, Guid storeId)
        {
            var productItem = new ProductItem
            {
                Name = productItemRecord.Name!.ToTitleCase().Trim(),
                Description = productItemRecord.Description!,
                ProductCategoryId = productItemRecord.ProductCategoryId!.Value,
                Price = productItemRecord.Price!.Value,
                IsEnable = productItemRecord.IsEnable!.Value,
                StoreId = storeId,
                CreatedAt = DateTime.Now,
                ImageUrl = productItemRecord.ImageUrl
            };
            _productItemRepo.Add(productItem);
            return productItem;
        }

        public ProductItem UpdateProductItem(ProductItem productItem, ProductItemRecord productItemRecord)
        {
            productItem.Name = productItemRecord.Name!.ToTitleCase().Trim();
            productItem.Description = productItemRecord.Description!;
            productItem.ProductCategoryId = productItemRecord.ProductCategoryId!.Value;
            productItem.Price = productItemRecord.Price!.Value;
            productItem.IsEnable = productItemRecord.IsEnable!.Value;
            productItem.ImageUrl = productItemRecord.ImageUrl;
            _productItemRepo.Update(productItem);
            return productItem;
        }

        public ProductItem DisableProductItem(ProductItem productItem)
        {
            productItem.IsEnable = false;
            _productItemRepo.Update(productItem);
            return productItem;
        }

        public ProductItem EnableProductItem(ProductItem productItem)
        {
            productItem.IsEnable = true;
            _productItemRepo.Update(productItem);
            return productItem;
        }

        public void DeleteProductItem(ProductItem productItem)
        {
            _productItemRepo.Delete(productItem);
        }
    }
}
