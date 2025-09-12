using FluentValidation;

namespace GymManagement.Application.Users.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull().WithMessage("Имя не может быть null.")
            .NotEmpty().WithMessage("Имя не может быть пустым.")
            .MaximumLength(50).WithMessage("Имя не может превышать 50 символов.");

        RuleFor(x => x.LastName)
            .NotNull().WithMessage("Фамилия не может быть null.")
            .NotEmpty().WithMessage("Фамилия не может быть пустой.")
            .MaximumLength(50).WithMessage("Фамилия не может превышать 50 символов.");

        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email не может быть null.")
            .NotEmpty().WithMessage("Email не может быть пустым.")
            .EmailAddress().WithMessage("Неверный формат email.")
            .MaximumLength(255).WithMessage("Email не может превышать 255 символов.");

        RuleFor(x => x.PhoneNumber)
            .NotNull().WithMessage("Номер телефона не может быть null.")
            .NotEmpty().WithMessage("Номер телефона обязателен.")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Неверный формат номера телефона. Должно быть 10-15 цифр с необязательным префиксом '+'.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Дата рождения не может быть пустой.")
            .LessThan(DateTime.Today).WithMessage("Дата рождения должна быть в прошлом.")
            .Must(BeAValidAge).WithMessage("Пользователь должен быть старше 13 лет.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Улица обязательна.")
            .MaximumLength(100).WithMessage("Улица не должна превышать 100 символов.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Город обязателен.")
            .MaximumLength(50).WithMessage("Город не должен превышать 50 символов.");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("Почтовый индекс обязателен.")
            .MaximumLength(20).WithMessage("Почтовый индекс не должен превышать 20 символов.");
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