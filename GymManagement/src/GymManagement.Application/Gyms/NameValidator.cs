using FluentValidation;
using GymManagement.Domain.Entities;

namespace GymManagement.Application.Gyms;

public class NameValidator : AbstractValidator<Name>
{
    public NameValidator()
    {
        RuleFor(n => n.Value)
            .NotEmpty().WithMessage("Name is mandatory for creating the gym")
            .Length(1, 100).WithMessage("Name must be from 1 to 100 symbols");
    }
}