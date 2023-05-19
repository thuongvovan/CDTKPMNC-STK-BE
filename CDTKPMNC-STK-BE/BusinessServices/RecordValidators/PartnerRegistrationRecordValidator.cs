using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class PartnerRegistrationRecordValidator : AbstractValidator<PartnerRegistrationRecord>
    {

        public PartnerRegistrationRecordValidator(AddressService addressService, CompanyService companyService)
        {
            RuleFor(partner => partner.Account)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new AccountRegistrationRecordValidator(addressService));

            RuleFor(partner => partner.PartnerType)
                .NotNull().NotEmpty().WithMessage("Specify individual partner or business partner.")
                .Must(type => Enum.GetNames(typeof(PartnerType)).ToList().Contains(type!.Value.ToString()))
                .WithMessage("{PropertyName} {PropertyValue} is invalid.");

            RuleFor(partner => partner.Company)
                .Must((partner, company) => !(partner!.PartnerType == PartnerType.Company && company == null))
                .WithMessage("{PropertyName} is required.")
                .SetValidator(new CompanyRecordValidator(addressService, companyService));
        }
    }
}


/*
sử dụng
var person = new Person { Name = "", Age = 15 };
var validator = new PersonValidator();
var result = validator.Validate(person);
if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine(error.ErrorMessage);
    }
}
 */