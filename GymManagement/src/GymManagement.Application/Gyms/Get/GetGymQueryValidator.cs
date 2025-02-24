using FluentValidation;

namespace GymManagement.Application.Gyms.Get;

public class GetGymQueryValidator : AbstractValidator<GetGymQuery>
{
    public GetGymQueryValidator()
    {
        RuleFor(x => x.GymId).NotEmpty();
    }
}