using CDTKPMNC_STK_BE.BusinessServices.Records;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class VerifyResetPasswordRecordValidator : AbstractValidator<VerifyResetPasswordRecord>
    {
        public VerifyResetPasswordRecordValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(vrs => vrs.UserName)
                .NotNull().NotEmpty().WithMessage("UserName is required.")
                .EmailAddress().WithMessage("UserName is not valid (email required).");

            RuleFor(vrs => vrs.Otp)
                .NotNull().NotEmpty().WithMessage("OTP is required.")
                .InclusiveBetween(100000, 999999).WithMessage("Invalid OTP.");
        }
    }
}
