using FluentValidation;
using GymManagement.Domain.Entities;

namespace GymManagement.Application.Gyms.Create.Validators;

public class DescriptionValidator : AbstractValidator<Description>
{
    public DescriptionValidator()
    {
        RuleFor(d => d.Value)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}