using CDTKPMNC_STK_BE.BusinessServices.Records;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.RecordValidators
{
    public class TokenRecordValidator : AbstractValidator<TokenRecord>
    {
        public TokenRecordValidator() 
        {
            RuleFor(add => add.Token)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not empty.");
        }
    }
}
