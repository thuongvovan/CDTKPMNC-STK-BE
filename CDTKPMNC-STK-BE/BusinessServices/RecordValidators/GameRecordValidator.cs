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
            RuleLevelCascadeMode = CascadeMode.Stop;

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
                .NotNull().NotEmpty().WithMessage("{PropertyName} is require.")
                .Must(i => i!.StartsWith('/')).WithMessage("{PropertyName} must be start with '/'.");
        }
    }
}

