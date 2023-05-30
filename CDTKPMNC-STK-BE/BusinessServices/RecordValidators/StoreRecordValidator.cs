using CDTKPMNC_STK_BE.BusinessServices.Records;
using FluentValidation;
using System;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class StoreRecordValidator : AbstractValidator<StoreRecord>
    {
        public StoreRecordValidator(AddressService addressService)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(store => store.Name)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(store => store.Description)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(store => store.Address)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new AddressRecordValidator(addressService));


            RuleFor(store => store.OpenTime)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new TimeRecordValidator());

            RuleFor(store => store.CloseTime)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new TimeRecordValidator());

            RuleFor(store => store.OpenTime)
                .Must((store, openTime) =>
                {
                    var open = new TimeSpan(openTime!.Hour!.Value, openTime!.Minute!.Value, 0);
                    var close = new TimeSpan(store.CloseTime!.Hour!.Value, store.CloseTime!.Minute!.Value, 0);
                    if (close > open) return true;
                    return false;
                }).WithMessage("CloseTime must be after OpenTime.");

            RuleFor(store => store.IsEnable)
                .NotNull().NotEmpty().WithMessage("Specify whether to enable or not.");

            RuleFor(store => store.BannerUrl)
                .NotEmpty().When(store => store.BannerUrl != null)
                .WithMessage("{PropertyName} is not empty.")
                .Must(i => i!.StartsWith('/')).WithMessage("{PropertyName} must be start with '/'.");
        }
    }
}