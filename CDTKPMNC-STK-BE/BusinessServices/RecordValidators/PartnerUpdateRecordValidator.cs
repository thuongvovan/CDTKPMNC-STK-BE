﻿using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class PartnerUpdateRecordValidator : AbstractValidator<PartnerUpdateRecord>
    {
        public PartnerUpdateRecordValidator(AddressService addressService, CompanyService companyService)
        {
            RuleFor(partner => partner.AccountUpdate)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new AccountUpdateRecordValidator(addressService));

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
