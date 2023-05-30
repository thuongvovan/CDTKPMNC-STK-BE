using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.RecordValidators;
using CDTKPMNC_STK_BE.Utilities;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class CampaignCreateRecordValidator : AbstractValidator<CampaignCreateRecord>
    {
        public CampaignCreateRecordValidator(GameService gameService, VoucherService voucherService) 
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(cp => cp.CampaignInfo)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new CampaignInfoRecordValidator(gameService));

            RuleFor(cp => cp.CampaignVoucherSeriesList)
                .NotNull().WithMessage("{PropertyName} is required.")
                .Must(vouchers => vouchers!.Length > 0)
                .WithMessage("List {PropertyName} must contain at least 1.");

            RuleForEach(cp => cp.CampaignVoucherSeriesList)
                .NotNull().WithMessage("Vouchers {CollectionIndex} is required.")
                .SetValidator(new CampaignVoucherSeriesRecordValidator(voucherService));
        }
    }
}


