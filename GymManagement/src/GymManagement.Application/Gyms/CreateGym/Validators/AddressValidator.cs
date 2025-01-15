using FluentValidation;
using GymManagement.Domain.Entities;

namespace GymManagement.Application.Gyms.CreateGym.Validators;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(a => a.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(50).WithMessage("City cannot exceed 50 characters.");

        RuleFor(a => a.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(100).WithMessage("Street cannot exceed 100 characters.");

        RuleFor(a => a.ZipCode)
            .NotEmpty().WithMessage("ZipCode is required.")
            .Matches(@"^\d{5}$").WithMessage("ZipCode must be a 5-digit number.");
    }
}