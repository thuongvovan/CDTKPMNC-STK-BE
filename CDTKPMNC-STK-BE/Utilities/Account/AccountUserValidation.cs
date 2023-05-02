using CDTKPMNC_STK_BE.Models;
using FluentValidation;

namespace CDTKPMNC_STK_BE.Utilities.Account
{
    public class AccountUserValidation : AbstractValidator<RegisteredAccount>
    {
        public AccountUserValidation()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .EmailAddress().WithMessage("UserName is not valid (email required).");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit.")
                .Matches("[a-z]").WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit.")
                .Matches("[A-Z]").WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit.")
                .Matches("[0-9]").WithMessage("Password must be at least 8 characters long, contain lowercase, uppercase and digit.");

/*            RuleFor(user => user.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Must((user, confirmPassword) => confirmPassword == user.Password)
                    .WithMessage("Passwords do not match.");
*/
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(user => user.Gender)
                .NotNull().WithMessage("Name is required.");

            RuleFor(user => user.BirthYear)
                .NotNull().WithMessage("BirthYear is required.")
                .InclusiveBetween(1900, DateTime.Now.Year).WithMessage("The year of birth must be between 1900 and the current year.");

            RuleFor(user => user.BirthMonth)
                .NotNull().WithMessage("BirthMonth is required.")
                .InclusiveBetween(1, 12).WithMessage("The month of birth must be between 1 and 12.");

            RuleFor(user => user.BirthDate)
                .NotEmpty().WithMessage("BirthDate is required.")
                .Must((user, BirthDate) => BirthDate >= 1 && BirthDate <= DateTime.DaysInMonth(user.BirthYear, user.BirthMonth))
                    .WithMessage("Date does not match month and year.");

            /*RuleFor(user => new { user.BirthDate, user.BirthMonth, user.BirthYear })
                .Must(DoB => DoB.BirthDate < 1 || DoB.BirthDate > DateTime.DaysInMonth(DoB.BirthYear, DoB.BirthMonth))
                    .WithMessage("Date does not match month and year.");*/






        }
    }
}


/*
sử dụng
var person = new Person { Name = "", Age = 15 };
var validator = new PersonValidator();
var result = validator.Validate(person);
if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine(error.ErrorMessage);
    }
}
 */