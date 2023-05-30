using FluentValidation;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class ResetPasswordRecordValidator : AbstractValidator<ResetPasswordRecord>
    {
        public ResetPasswordRecordValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(rs => rs.UserName)
                .NotNull().NotEmpty().WithMessage("UserName is required.")
                .EmailAddress().WithMessage("UserName is not valid (email required).");

            RuleFor(rs => rs.NewPassword)
                .NotNull().NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit..")
                .Matches("[a-z]").WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit..")
                .Matches("[A-Z]").WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit..")
                .Matches("[0-9]").WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit..");
        }
    }
}
