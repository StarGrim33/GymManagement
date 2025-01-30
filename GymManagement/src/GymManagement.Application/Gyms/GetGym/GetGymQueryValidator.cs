using FluentValidation;

namespace GymManagement.Application.Gyms.GetGym;

public class GetGymQueryValidator : AbstractValidator<GetGymQuery>
{
    public GetGymQueryValidator()
    {
        RuleFor(x => x.GymId).NotEmpty();
    }
}