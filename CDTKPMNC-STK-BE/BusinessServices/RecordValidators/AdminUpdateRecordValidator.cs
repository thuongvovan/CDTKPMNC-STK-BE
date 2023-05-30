using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class AdminUpdateRecordValidator : AbstractValidator<AdminUpdateRecord?>
    {
        public AdminUpdateRecordValidator(AddressService addressService) 
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(accUpdate => accUpdate!.AccountUpdate)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new AccountUpdateRecordValidator(addressService));

            RuleFor(accUpdate => accUpdate!.Position)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(accUpdate => accUpdate!.Department)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
