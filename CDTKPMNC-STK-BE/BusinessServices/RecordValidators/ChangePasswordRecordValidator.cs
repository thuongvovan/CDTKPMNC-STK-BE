using FluentValidation;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class ChangePasswordRecordValidator : AbstractValidator<ChangePasswordRecord>
    {
        public ChangePasswordRecordValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(user => user.OldPassword)
                .NotNull().NotEmpty().WithMessage("Your current password is required.");

            RuleFor(user => user.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters, contain lowercase, uppercase and digit..")
                .Matches("[a-z]").WithMessage("Password must be at least 8 characters, contain lowercase, uppercase and digit..")
                .Matches("[A-Z]").WithMessage("Password must be at least 8 characters, contain lowercase, uppercase and digit..")
                .Matches("[0-9]").WithMessage("Password must be at least 8 characters, contain lowercase, uppercase and digit..");
                
        }
    }
}
