using FluentValidation;

namespace GymManagement.Application.Memberships.BuyMembership;

public class BuyMembershipCommandValidator : AbstractValidator<BuyMembershipCommand>
{
    public BuyMembershipCommandValidator()
    {
        RuleFor(x => x.GymId).NotEmpty();

        RuleFor(x => x.MembershipTypeId).NotEmpty();

        RuleFor(x => x.UserId).NotEmpty();
    }
}