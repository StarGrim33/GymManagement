using FluentValidation;

namespace GymManagement.Application.MembershipTypes.SearchMembershipTypes;

public class SearchMembershipTypesByNameValidator : AbstractValidator<SearchMembershipTypesByNameQuery>
{
    public SearchMembershipTypesByNameValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Membership type is mandatory for search");
    }
}