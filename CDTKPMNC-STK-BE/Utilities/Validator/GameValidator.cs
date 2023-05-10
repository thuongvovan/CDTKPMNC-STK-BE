using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Repositories;
using FluentValidation;
using System;

namespace CDTKPMNC_STK_BE.Utilities.Validator
{
    public class GameValidator : AbstractValidator<GameInfo>
    {
        public GameValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(g => g.Name)
                .NotNull().NotEmpty()
                .WithMessage("Game name is required.");

            RuleFor(g => g.Description)
                .NotNull().NotEmpty()
                .WithMessage("Description is required.");

            RuleFor(g => g.Instruction)
                .NotNull().NotEmpty()
                .WithMessage("Instruction is required.");

            RuleFor(g => g.IsEnable)
                .NotNull().WithMessage("IsEnable is true or false.");

        }
    }
}

