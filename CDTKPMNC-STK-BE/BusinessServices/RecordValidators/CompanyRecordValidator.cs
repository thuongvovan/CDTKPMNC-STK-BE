using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.Records;

using FluentValidation;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class CompanyRecordValidator : AbstractValidator<CompanyRecord?>
    {
        public CompanyRecordValidator(AddressService addressService, CompanyService companyService)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(com => com!.Name)
                .NotNull().NotEmpty().WithMessage("{PropertyName} name is required.")
                .Must(name => companyService.GetByName(name!) == null).WithMessage("{PropertyValue} name is really existed.");

            RuleFor(com => com!.BusinessCode)
                .NotNull().NotEmpty().WithMessage("{PropertyName} Code is required.")
                .Must(code => companyService.GetByBusinessCode(code!) == null).WithMessage("{PropertyValue} Business Code is really existed.");

            RuleFor(com => com!.Address)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .SetValidator(new AddressRecordValidator(addressService));
        }
    }

    public class CompanyRecordUpdateValidator : AbstractValidator<CompanyRecord?>
    {
        public CompanyRecordUpdateValidator(AccountPartner currentPartner, AddressService addressService, CompanyService companyService)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(com => com!.Name)
                .NotNull().NotEmpty().WithMessage("{PropertyName} name is required.")
                .Must((name) => {
                    var company = companyService.GetByName(name!);
                    if (company == null || company.Id == currentPartner.Id)
                    {
                        return true;
                    }   
                    return false;
                }).WithMessage("{PropertyValue} name is really existed.");

            RuleFor(com => com!.BusinessCode)
                .NotNull().NotEmpty().WithMessage("{PropertyName} Code is required.")
                .Must(code => { 
                    var company = companyService.GetByBusinessCode(code!);
                    if (company == null || company.Id == currentPartner.Id)
                    {
                        return true;
                    }
                    return false;
                }).WithMessage("{PropertyValue} Business Code is really existed.");

            RuleFor(com => com!.Address)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .SetValidator(new AddressRecordValidator(addressService));
        }
    }
}

