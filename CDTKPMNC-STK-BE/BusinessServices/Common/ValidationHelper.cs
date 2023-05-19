using FluentValidation.Results;

namespace CDTKPMNC_STK_BE.BusinessServices.Common
{
    public class ValidationSummary
    {
        public ValidationSummary(bool isValid, string? errorMessage = null)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; } = null;
    }

    static public class ValidationHelper
    {
        static public ValidationSummary GetSummary(this ValidationResult result)
        {
            if (!result.IsValid)
            {
                var ErrorMessage = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                return new ValidationSummary(false, ErrorMessage);
            }
            return new ValidationSummary(true);
        }
    }
}
