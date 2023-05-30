using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class AccountUpdateRecordValidator : AbstractValidator<AccountUpdateRecord?>
    {
        public AccountUpdateRecordValidator(AddressService addressService)
        {

            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(user => user!.Name)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(user => user!.Gender)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.")
                .Must(gender => Enum.GetNames(typeof(Gender)).ToList().Contains(gender!.Value.ToString()));

            RuleFor(user => user!.Address)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .SetValidator(new AddressRecordValidator(addressService));

            RuleFor(user => user!.DateOfBirth)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new DateRecordValidator());
        }
    }
}
