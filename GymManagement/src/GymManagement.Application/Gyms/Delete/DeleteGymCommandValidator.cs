using FluentValidation;

namespace GymManagement.Application.Gyms.Delete;

public class DeleteGymCommandValidator : AbstractValidator<DeleteGymCommand>
{
    public DeleteGymCommandValidator()
    {
        RuleFor(command => command.GymId)
            .NotEmpty().WithMessage("GymId must not be empty.");
    }
}