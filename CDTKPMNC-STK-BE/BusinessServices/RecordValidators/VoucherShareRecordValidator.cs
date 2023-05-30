using CDTKPMNC_STK_BE.BusinessServices.Records;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class VoucherShareRecordValidator : AbstractValidator<VoucherShareRecord>
    {
        public VoucherShareRecordValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(vs => vs.DestinationUser)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not empty.")
                .EmailAddress().WithMessage("recipient account must be an email");

            RuleFor(vs => vs.VoucherCode)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not empty.");
        }
    }
}
