using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class CampaignInfoRecordValidator : AbstractValidator<CampaignInfoRecord?>
    {
        public CampaignInfoRecordValidator(GameService gameService) 
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(cp => cp!.Name)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(cp => cp!.Description)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(cp => cp!.StartDate)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new DateRecordValidator())
                .Must(startDate => startDate!.ToDateTime() >= DateTime.Now.Date )
                .WithMessage("{PropertyName} must be after current date.");

            RuleFor(cp => cp!.EndDate)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new DateRecordValidator())
                .Must(endDate => endDate!.ToDateTime() >= DateTime.Now.Date)
                .WithMessage("{PropertyName} must be after current date.")
                .Must((cp, endDate) => cp!.StartDate!.ToDateTime() < endDate!.ToDateTime())
                .WithMessage("End date must be after the start date");

            RuleFor(cp => cp!.GameId)
                .NotNull().WithMessage("{PropertyName} is required.")
                .Must(gameId => gameService.GetById(gameId!.Value) != null)
                .WithMessage("Incorrect {PropertyName}");

            RuleFor(cp => cp!.WinRate)
                .NotNull().WithMessage("{PropertyName} is required.")
                .InclusiveBetween(0, 100).WithMessage("{PropertyName} required inclusive between 0 and 100.");

            RuleFor(cp => cp!.IsEnable)
                .NotNull().WithMessage("{PropertyName} is required.");

            RuleFor(cp => cp!.GameRule)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.")
                .Must(gameRule => Enum.GetNames(typeof(GameRule)).ToList().Contains(gameRule!.Value.ToString()))
                .WithMessage("{PropertyName} {PropertyValue} is invalid.");

            RuleFor(cp => cp!.NumberOfLimit)
                .NotNull().When(cp => cp!.GameRule == GameRule.Limit)
                .WithMessage("{PropertyName} is required.")
                .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} >= 1 is required.")
                .When(cp => cp!.GameRule == GameRule.Limit);
        }
    }
}
