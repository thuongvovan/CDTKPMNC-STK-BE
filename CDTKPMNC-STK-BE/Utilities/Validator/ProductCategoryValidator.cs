using FluentValidation;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Utilities.Validator
{
    public class ProductCategoryValidator : AbstractValidator<ProductCategoryInfo>
    {
        public ProductCategoryValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(cat => cat.Name)
                .NotNull().WithMessage("Product category name is required.");

            RuleFor(cat => cat.Description)
                .NotNull().WithMessage("Description is required.");

            RuleFor(cat => cat.IsEnable)
                .NotNull().WithMessage("IsEnable is required.");
        }
    }
}
