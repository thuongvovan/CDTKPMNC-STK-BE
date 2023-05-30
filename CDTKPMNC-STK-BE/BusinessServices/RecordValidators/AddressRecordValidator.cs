using FluentValidation;
using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class AddressRecordValidator : AbstractValidator<AddressRecord?>
    {
        
        public AddressRecordValidator(AddressService addressService) 
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(add => add!.WardId)
                .NotNull().NotEmpty().WithMessage("WardId is required.")
                .Must(wardId => int.TryParse(wardId, out int id) && addressService!.GetWardById(wardId) != null)
                .WithMessage("Your address is incorect.");

            RuleFor(add => add!.Street)
               .NotEmpty().WithMessage("Street is not empty.");
        }
    }
}
