using FluentValidation;

namespace GymManagement.Application.Gyms.CreateGym.Validators;

public class CreateGymCommandValidator : AbstractValidator<CreateGymCommand>
{
    public CreateGymCommandValidator()
    {
        RuleFor(x => x.Name)
            .SetValidator(new NameValidator());

        RuleFor(x => x.Description)
            .SetValidator(new DescriptionValidator());

        RuleFor(x => x.Address)
            .SetValidator(new AddressValidator());

        RuleFor(x => x.Schedule)
            .SetValidator(new ScheduleValidator());
    }
}