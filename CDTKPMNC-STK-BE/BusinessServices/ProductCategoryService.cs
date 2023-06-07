using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.RecordValidators;
using CDTKPMNC_STK_BE.Utilities;

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class ProductCategoryService : CommonService
    {
        private readonly IProductCategoryRepository _productCategoryRepo;

        public ProductCategoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _productCategoryRepo = _unitOfWork.ProductCategoryRepo;
        }

        public ValidationSummary ValidateProductCategoryRecord(ProductCategoryRecord productCategoryRecord)
        {
            if (productCategoryRecord == null)
            {
                return new ValidationSummary(false, "Product Category infomation is required.");
            }
            var validator = new ProductCategoryRecordValidator();
            var result = validator.Validate(productCategoryRecord);
            return result.GetSummary();
        }

        public bool VerifyProductCategoryRecord(ProductCategoryRecord productCategoryRecord)
        {
            var productCategory = _productCategoryRepo.GetByName(productCategoryRecord.Name!);
            if (productCategory == null)
            {
                return true;
            }
            return false;
        }

        public ProductCategory CreateProductCategory(ProductCategoryRecord productCategoryRecord)
        {
            var productCategory = new ProductCategory
            {
                Name = productCategoryRecord.Name!.ToTitleCase(),
                Description = productCategoryRecord.Description!,
                IsEnable = productCategoryRecord.IsEnable!.Value,
            };
            _productCategoryRepo.Add(productCategory);
            return productCategory;
        }

        public List<ProductCategory> GetAllProductCategory()
        {
            var productCategories = _productCategoryRepo.GetAll();
            if (productCategories != null)
            {
                return productCategories.ToList();
            }
            return new List<ProductCategory>(0);
        }

        public List<ProductCategory> GetAvalibleProductCategory()
        {
            var productCategories = _productCategoryRepo.GetAvalible();
            if (productCategories != null)
            {
                return productCategories.ToList();
            }
            return new List<ProductCategory>(0);
        }

        public List<ProductCategory> GetDisabledProductCategory()
        {
            var productCategories = _productCategoryRepo.GetDisabled();
            if (productCategories != null)
            {
                return productCategories.ToList();
            }
            return new List<ProductCategory>(0);
        }

        public ProductCategory? GetProductCategory(Guid productCategoryId)
        {
            var productCategory = _productCategoryRepo.GetById(productCategoryId);
            return productCategory;
        }

        public bool DeleteProductCategory(Guid productCategoryId)
        {
            var productCategory = _productCategoryRepo.GetById(productCategoryId);
            if (productCategory != null && (productCategory.ProductItems == null || productCategory.ProductItems.Count == 0))
            {
                _productCategoryRepo.Delete(productCategory);
                return true;
            }
            return false;
        }

        public ProductCategory? UpdateProductCategory(Guid productCategoryId, ProductCategoryRecord productCategoryRecord)
        {
            var productCategory = _productCategoryRepo.GetById(productCategoryId);
            if (productCategory != null)
            {
                productCategory.Name = productCategoryRecord.Name!;
                productCategory.Description = productCategoryRecord.Description!;
                productCategory.IsEnable = productCategoryRecord.IsEnable!.Value;
                _productCategoryRepo.Update(productCategory);
                return productCategory;
            }
            return null;
        }

        public ProductCategory? DisableProductCategory(Guid productCategoryId)
        {
            var productCategory = _productCategoryRepo.GetById(productCategoryId);
            if (productCategory != null)
            {
                productCategory.IsEnable = false;
                _productCategoryRepo.Update(productCategory);
                return productCategory;
            }
            return null;
        }

        public ProductCategory? EnableProductCategory(Guid productCategoryId)
        {
            var productCategory = _productCategoryRepo.GetById(productCategoryId);
            if (productCategory != null)
            {
                productCategory.IsEnable = true;
                _productCategoryRepo.Update(productCategory);
                return productCategory;
            }
            return null;
        }

        #region for Dasboard

        public int CountAll()
        {
            return _productCategoryRepo.GetAll().Count();
        }

        #endregion
    }
}
