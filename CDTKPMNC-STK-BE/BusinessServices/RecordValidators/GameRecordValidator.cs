using CDTKPMNC_STK_BE.BusinessServices.Records;
using FluentValidation;
using System;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class GameRecordValidator : AbstractValidator<GameRecord>
    {
        public GameRecordValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(g => g.Name)
                .NotNull().NotEmpty()
                .WithMessage("{PropertyName} is required.");

            RuleFor(g => g.Description)
                .NotNull().NotEmpty()
                .WithMessage("{PropertyName} is required.");

            RuleFor(g => g.Instruction)
                .NotNull().NotEmpty()
                .WithMessage("{PropertyName} is required.");

            RuleFor(g => g.IsEnable)
                .NotNull().WithMessage("{PropertyName} is true or false.");

            RuleFor(g => g.ImageUrl)
                .NotEmpty().When(g => g.ImageUrl != null)
                .WithMessage("{PropertyName} is not empty.");
        }
    }
}

