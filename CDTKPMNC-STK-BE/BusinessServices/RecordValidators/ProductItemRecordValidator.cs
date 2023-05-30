using CDTKPMNC_STK_BE.BusinessServices.Records;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class ProductItemRecordValidator : AbstractValidator<ProductItemRecord>
    {
        public ProductItemRecordValidator(ProductCategoryService productCategoryService) 
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(item => item.Name)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(item => item.Description)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(item => item.Price)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} >= 0 required.");

            RuleFor(item => item.ProductCategoryId)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.")
                .Must(categoryId => productCategoryService.GetProductCategory(categoryId!.Value) != null)
                .WithMessage("{PropertyName} does not exist.");

            RuleFor(item => item.IsEnable)
                .NotNull().WithMessage("{PropertyName} is required.");

            RuleFor(item => item.ImageUrl)
                .NotEmpty().When(item => item.ImageUrl != null)
                .WithMessage("{PropertyName} is not empty.")
                .Must(i => i!.StartsWith('/')).WithMessage("{PropertyName} must be start with '/'.");
        }
    }
}
