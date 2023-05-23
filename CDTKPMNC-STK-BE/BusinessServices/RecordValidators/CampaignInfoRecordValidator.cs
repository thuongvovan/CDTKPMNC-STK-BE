using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Utilities;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class CampaignInfoRecordValidator : AbstractValidator<CampaignInfoRecord?>
    {
        public CampaignInfoRecordValidator(GameService gameService) 
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

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

            RuleFor(cp => cp!.IsEnable)
                .NotNull().WithMessage("{PropertyName} is required.");
        }
    }
}
