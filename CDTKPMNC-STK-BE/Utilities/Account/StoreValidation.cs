using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Repositories;
using FluentValidation;
using System;

namespace CDTKPMNC_STK_BE.Utilities.AccountUtils
{
    public class StoreValidation : AbstractValidator<StoreRegistrationInfo>
    {
        public StoreValidation()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(store => store.Name)
                .NotNull().WithMessage("Company name is required.");

            RuleFor(store => store.Description)
                .NotNull().WithMessage("Description is required.");

            RuleFor(store => store.Address)
                .NotNull().WithMessage("Address is required.");

            RuleFor(store => store.Address.WardId)
                .NotNull().WithMessage("WardId is required.")
                .Must(wardId => int.TryParse(wardId, out int id))
                .WithMessage("Invalid wardId.");

            RuleFor(store => store.Address.Street)
                .NotNull().WithMessage("Street is required.");

            RuleFor(store => store.OpenTime.Hours)
                .NotNull().WithMessage("Open Hours is required.")
                .InclusiveBetween(0, 23).WithMessage("Hours is between 0 and 23");

            RuleFor(store => store.OpenTime.Minute)
                .NotNull().WithMessage("Open Minute is required.")
                .InclusiveBetween(0, 59).WithMessage("Minute is between 0 and 59");

            RuleFor(store => store.CloseTime.Hours)
                .NotNull().WithMessage("Close Hours is required.")
                .InclusiveBetween(0, 23).WithMessage("Hours is between 0 and 23");

            RuleFor(store => store.CloseTime.Minute)
                .NotNull().WithMessage("Close Minute is required.")
                .InclusiveBetween(0, 59).WithMessage("Minute is between 0 and 59");

            RuleFor(store => store.OpenTime)
                .Must((store, openTime) =>
                {
                    var open = new TimeSpan(openTime.Hours, openTime.Minute, 0);
                    var close = new TimeSpan(store.CloseTime.Hours, store.CloseTime.Minute, 0);
                    if (close > open) return true;
                    return false;
                }).WithMessage("Close Minute is required.");

            RuleFor(store => store.IsEnable)
                .NotNull().WithMessage("Specify whether to enable or not.");
        }
    }
}

