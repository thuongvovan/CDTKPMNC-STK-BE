using CDTKPMNC_STK_BE.BusinessServices.Records;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class DateRecordValidator : AbstractValidator<DateRecord?>
    {
        public DateRecordValidator() 
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(d => d!.Year)
                .NotNull().NotEmpty().WithMessage("Year is required.")
                .GreaterThanOrEqualTo(1900).WithMessage("The year must be greater than or equal to 1900");

            RuleFor(d => d!.Month)
                .NotNull().NotEmpty().WithMessage("Month is required.")
                .InclusiveBetween(1, 12).WithMessage("The month must be between 1 and 12.");

            RuleFor(d => d!.Day)
                .NotNull().NotEmpty().WithMessage("Day is required.")
                .Must((d, BirthDate) => BirthDate >= 1 && BirthDate <= DateTime.DaysInMonth(d!.Year!.Value, d!.Month!.Value))
                .WithMessage("Date is not correct.");
        }
    }
}
