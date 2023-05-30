using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class CampaignVoucherSeriesRecordValidator : AbstractValidator<CampaignVoucherSeriesRecord>
    {
        public CampaignVoucherSeriesRecordValidator(VoucherService voucherService)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(vsc => vsc.VoucherSeriesId)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.")
                .Must(voucherSeriesId => voucherService.GetVoucherSeries(voucherSeriesId!.Value) != null)
                .WithMessage("{PropertyName} {PropertyValue} is not exist.");

            RuleFor(vsc => vsc.Quantity)
               .NotNull().WithMessage("{PropertyName} is required.")
               .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} >=0 required.");

            RuleFor(vs => vs.ExpiresOn)
                .NotNull().WithMessage("{PropertyName} is required.")
                .SetValidator(new DateRecordValidator())
                .Must(expiresOn => new DateTime(expiresOn!.Year!.Value, expiresOn!.Month!.Value, expiresOn!.Day!.Value) > DateTime.Now)
                .WithMessage("{PropertyName} must be after current date.");
        }
    }
}