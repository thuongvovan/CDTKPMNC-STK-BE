using CDTKPMNC_STK_BE.BusinessServices.Records;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class TimeRecordValidator : AbstractValidator<TimeRecord?>
    {
        public TimeRecordValidator() 
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(t => t!.Minute)
                .NotNull().WithMessage("{{PropertyName}} is required.")
                .InclusiveBetween(0, 59).WithMessage("{PropertyName} must be between 0 and 59");

            RuleFor(t => t!.Hour)
                .NotNull().WithMessage(" {{PropertyName}} is required.")
                .InclusiveBetween(0, 23).WithMessage("{PropertyName} must be between 0 and 23");
        }
    }
}
