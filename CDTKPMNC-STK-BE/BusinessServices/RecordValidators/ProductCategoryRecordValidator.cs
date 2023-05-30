using FluentValidation;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class ProductCategoryRecordValidator : AbstractValidator<ProductCategoryRecord>
    {
        public ProductCategoryRecordValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(cat => cat.Name)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(cat => cat.Description)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(cat => cat.IsEnable)
                .NotNull().WithMessage("{PropertyName} is required.");
        }
    }
}
