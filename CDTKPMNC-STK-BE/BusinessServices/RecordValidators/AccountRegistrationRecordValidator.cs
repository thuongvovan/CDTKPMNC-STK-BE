using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class AccountRegistrationRecordValidator : AbstractValidator<AccountRegistrationRecord?>
    {
        public AccountRegistrationRecordValidator(AddressService addressService) 
        {
            RuleFor(user => user!.UserName)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("{PropertyName} is not valid (email required).");

            RuleFor(user => user!.Password)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit.")
                .Matches("[a-z]").WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit.")
                .Matches("[A-Z]").WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit.")
                .Matches("[0-9]").WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit.");

            RuleFor(user => user!.Name)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(user => user!.Gender)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.")
                .Must(gender => Enum.GetNames(typeof(Gender)).ToList().Contains(gender!.Value.ToString()));

            RuleFor(user => user!.Address)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .SetValidator(new AddressRecordValidator(addressService));

            RuleFor(user => user!.BirthDate)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new DateRecordValidator());
        }
    }
}
