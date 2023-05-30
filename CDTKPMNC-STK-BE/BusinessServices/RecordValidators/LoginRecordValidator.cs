using FluentValidation;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class LoginRecordValidator : AbstractValidator<LoginRecord>
    {
        public LoginRecordValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(user => user.UserName)
               .NotNull().NotEmpty().WithMessage("UserName is required.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
