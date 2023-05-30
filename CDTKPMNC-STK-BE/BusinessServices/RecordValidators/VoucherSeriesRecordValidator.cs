using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Utilities;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class VoucherSeriesRecordValidator : AbstractValidator<VoucherSeriesRecord>
    {
        
        public VoucherSeriesRecordValidator() 
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(vs => vs.Name)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(vs => vs.Description)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
