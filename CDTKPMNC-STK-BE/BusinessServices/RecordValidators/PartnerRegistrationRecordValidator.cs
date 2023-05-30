using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class PartnerRegistrationRecordValidator : AbstractValidator<PartnerRegistrationRecord>
    {

        public PartnerRegistrationRecordValidator(AddressService addressService, CompanyService companyService)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(partner => partner.Account)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new AccountRegistrationRecordValidator(addressService));

            RuleFor(partner => partner.PartnerType)
                .NotNull().NotEmpty().WithMessage("Specify individual partner or business partner.")
                .Must(type => Enum.GetNames(typeof(PartnerType)).ToList().Contains(type!.Value.ToString()))
                .WithMessage("{PropertyName} {PropertyValue} is invalid.");
            
            RuleFor(partner => partner.Company)
                .NotNull().When(partner => partner.PartnerType == PartnerType.Company)
                .WithMessage("{PropertyName} is required.")
                .SetValidator(new CompanyRecordValidator(addressService, companyService))
                .When(partner => partner.PartnerType == PartnerType.Company);
        }
    }
}