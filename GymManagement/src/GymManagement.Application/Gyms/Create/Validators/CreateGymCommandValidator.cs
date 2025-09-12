using FluentValidation;

namespace GymManagement.Application.Gyms.Create.Validators;

public class CreateGymCommandValidator : AbstractValidator<CreateGymCommand>
{
    public CreateGymCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название спортзала обязательно")
            .MaximumLength(100)
            .WithMessage("Название не должно превышать 100 символов");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Описание спортзала обязательно")
            .MaximumLength(1000)
            .WithMessage("Описание не должно превышать 1000 символов");

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Улица обязательна")
            .MaximumLength(100)
            .WithMessage("Улица не должна превышать 100 символов");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("Город обязателен")
            .MaximumLength(50)
            .WithMessage("Город не должен превышать 50 символов");

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage("Почтовый индекс обязателен")
            .MaximumLength(20)
            .WithMessage("Почтовый индекс не должен превышать 20 символов");

        RuleFor(x => x.Schedule)
            .NotEmpty()
            .WithMessage("Расписание обязательно");
    }
}