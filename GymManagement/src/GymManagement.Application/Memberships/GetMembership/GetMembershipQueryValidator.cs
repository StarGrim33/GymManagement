using FluentValidation;

namespace GymManagement.Application.Memberships.GetMembership;

public class GetMembershipQueryValidator : AbstractValidator<GetMembershipQuery>
{
    public GetMembershipQueryValidator()
    {
        RuleFor(x => x.MembershipId).NotEmpty();
    }
}