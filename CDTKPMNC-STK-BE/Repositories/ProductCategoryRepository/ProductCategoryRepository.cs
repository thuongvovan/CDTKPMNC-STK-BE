using CDTKPMNC_STK_BE.DatabaseContext;
using CDTKPMNC_STK_BE.Models;
using System.Linq;
using static CDTKPMNC_STK_BE.Controllers.ProductCategoryController;

namespace CDTKPMNC_STK_BE.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductCategoryRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public void Add(ProductCategory category)
        {
            if (category != null)
            {
                _dbContext.ProductCategories.Add(category);
                _dbContext.SaveChanges();
            }
        }

        public void Add(ProductCategoryInfo categoryInfo)
        {
            if (categoryInfo != null)
            {
                var productCategory = new ProductCategory
                {
                    Name = categoryInfo.Name,
                    Description = categoryInfo.Description,
                    IsEnable = categoryInfo.IsEnable,
                    CreatedAt = DateTime.Now,
                };
                _dbContext.ProductCategories.Add(productCategory);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(ProductCategory? category)
        {
            if (category != null)
            {
                _dbContext.ProductCategories.Remove(category);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(Guid productCategoryId)
        {
            ProductCategory? category = _dbContext.ProductCategories.Find(productCategoryId);
            Delete(category);
        }

        public List<ProductCategory> GetAll()
        {
            return _dbContext.ProductCategories.ToList();
        }

        public ProductCategory? GetById(Guid productCategoryId)
        {
            return _dbContext.ProductCategories.Find(productCategoryId);
        }

        public ProductCategory? GetByName(string name)
        {
            return _dbContext.ProductCategories.SingleOrDefault(c => c.Name.ToLower() == name.ToLower());
        }

        public List<ProductCategory> GetAvalible()
        {
            return _dbContext.ProductCategories.Where(c => c.IsEnable).ToList();
        }

        public void Update(ProductCategory category)
        {
            _dbContext.ProductCategories.Update(category);
            _dbContext.SaveChanges();
        }

        public void Update(ProductCategory category, ProductCategoryInfo productCategoryInfo)
        {
            category.Name = productCategoryInfo.Name;
            category.Description = productCategoryInfo.Description;
            category.IsEnable = productCategoryInfo.IsEnable;
            _dbContext.ProductCategories.Update(category);
            _dbContext.SaveChanges();
        }


        public void Enable(ProductCategory category)
        {
            category.IsEnable = true;
            _dbContext.SaveChanges();
        }
        public void Disable(ProductCategory category)
        {
            category.IsEnable = false;
            _dbContext.SaveChanges();
        }
    }
}
