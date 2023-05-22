using FluentValidation;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class ChangePasswordRecordValidator : AbstractValidator<ChangePasswordRecord>
    {
        public ChangePasswordRecordValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(user => user.OldPassword)
                .NotNull().NotEmpty().WithMessage("Your current password is required.");

            RuleFor(user => user.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8)
                .Matches("[a-z]")
                .Matches("[A-Z]")
                .Matches("[0-9]").WithMessage("Password must be at least 8 characters, contain lowercase, uppercase and digit..");
        }
    }
}
