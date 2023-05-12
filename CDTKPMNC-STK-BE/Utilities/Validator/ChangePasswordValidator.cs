﻿using FluentValidation;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Utilities.Validator
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordInfo>
    {
        public ChangePasswordValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(user => user.OldPassword)
                .NotEmpty().WithMessage("Your current password is required.");

            RuleFor(user => user.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[a-z]").WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit..")
                .Matches("[A-Z]").WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit..")
                .Matches("[0-9]").WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit..");

/*            RuleFor(user => user.NewPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Must((user, confirmPassword) => confirmPassword == user.ConfirmPassword)
                    .WithMessage("Passwords do not match.");*/
        }
    }
}
