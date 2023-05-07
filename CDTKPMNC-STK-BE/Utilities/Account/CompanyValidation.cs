﻿using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Repositories;
using FluentValidation;
using System;

namespace CDTKPMNC_STK_BE.Utilities.AccountUtils
{
    public class CompanyValidation : AbstractValidator<CompanyRegistrationInfo>
    {
        public CompanyValidation()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(com => com.Name)
                .NotNull().WithMessage("Company name is required.");

            RuleFor(com => com.BusinessCode)
                .NotNull().WithMessage("Business Code is required.");

            RuleFor(com => com.Address)
                .NotNull().WithMessage("Address is required.");

            RuleFor(com => com.Address.WardId)
                .NotNull().WithMessage("WardId is required.")
                .Must(wardId => int.TryParse(wardId, out int id))
                .WithMessage("Invalid wardId.");

            RuleFor(com => com.Address.Street)
                .NotNull().WithMessage("Street is required.");

        }
    }
}

