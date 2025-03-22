using FluentValidation;
using GymManagement.Application.Gyms.Create.Validators;

namespace GymManagement.Application.Users.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName.Value)
            .NotNull().WithMessage("First name cannot be null.")
            .NotEmpty().WithMessage("First name cannot be empty.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName.Value)
            .NotNull().WithMessage("Last name cannot be null.")
            .NotEmpty().WithMessage("Last name cannot be empty.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.Email.Value)
            .NotNull().WithMessage("Email cannot be null.")
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotNull().WithMessage("Phone number cannot be null.")
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number format. Must be 10-15 digits with optional '+' prefix.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth cannot be empty.")
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.")
            .Must(BeAValidAge).WithMessage("User must be at least 13 years old.");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Address cannot be null.")
            .SetValidator(new AddressValidator());

        RuleFor(x => x.Role.Id)
            .LessThan(2);
    }

    private static bool BeAValidAge(DateTime dateOfBirth)
    {
        const int minAge = 14;

        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > today.AddYears(-age))
        {
            age--;
        }

        return age >= minAge;
    }
}