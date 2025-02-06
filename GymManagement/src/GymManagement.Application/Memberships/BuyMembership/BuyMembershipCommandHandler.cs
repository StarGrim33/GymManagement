using GymManagement.Application.Abstractions.Exceptions;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Gyms.Errors;
using GymManagement.Domain.Entities.Gyms.QueryOptions;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Memberships.Errors;
using GymManagement.Domain.Entities.Memberships.MembershipTypes;
using GymManagement.Domain.Entities.Users;
using GymManagement.Domain.Entities.Users.Errors;

namespace GymManagement.Application.Memberships.BuyMembership;

internal sealed class BuyMembershipCommandHandler(
    IUserRepository userRepository,
    IMembershipTypeRepository membershipTypeRepository,
    IUnitOfWork unitOfWork,
    IGymRepository gymRepository,
    IMembershipRepository membershipRepository)
    : ICommandHandler<BuyMembershipCommand, Guid>
{
    public async Task<Result<Guid>> Handle(BuyMembershipCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        // Получить тип членства
        var membershipType = await membershipTypeRepository.GetByIdAsync(request.MembershipTypeId, cancellationToken);

        if (membershipType is null)
        {
            return Result.Failure<Guid>(MembershipErrors.NotFound);
        }

        var gymQueryOptions = new GymQueryOptions
        {
            IncludeMemberships = true,
        };

        var gym = await gymRepository.GetAsync(g => g.Id == request.GymId, gymQueryOptions, cancellationToken);

        if (gym is null)
        {
            return Result.Failure<Guid>(GymErrors.NotFound);
        }

        try
        {
            // Есть ли у пользователя активный абонемент или это новая покупка
            var purchaseResult = Membership.Buy(membershipType, user, gym);

            if (purchaseResult.IsNewMembership)
            {
                membershipRepository.AddAsync(purchaseResult.Membership);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(purchaseResult.Membership.Id);
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(MembershipErrors.Overlap);
        }
    }
}