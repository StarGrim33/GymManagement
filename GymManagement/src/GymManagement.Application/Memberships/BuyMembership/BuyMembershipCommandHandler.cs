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

internal sealed class BuyMembershipCommandHandler : ICommandHandler<BuyMembershipCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IMembershipTypeRepository _membershipTypeRepository;
    private readonly IMembershipRepository _membershipRepository;
    private readonly IGymRepository _gymRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BuyMembershipCommandHandler(
        IUserRepository userRepository, 
        IMembershipTypeRepository membershipTypeRepository, 
        IUnitOfWork unitOfWork, IGymRepository gymRepository, IMembershipRepository membershipRepository)
    {
        _userRepository = userRepository;
        _membershipTypeRepository = membershipTypeRepository;
        _unitOfWork = unitOfWork;
        _gymRepository = gymRepository;
        _membershipRepository = membershipRepository;
    }

    public async Task<Result<Guid>> Handle(BuyMembershipCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        // Получить тип членства
        var membershipType = await _membershipTypeRepository.GetByIdAsync(request.MembershipTypeId, cancellationToken);

        if (membershipType is null)
        {
            return Result.Failure<Guid>(MembershipErrors.NotFound);
        }

        var gymQueryOptions = new GymQueryOptions
        {
            IncludeMemberships = true,
        };

        var gym = await _gymRepository.GetAsync(g => g.Id == request.GymId, gymQueryOptions, cancellationToken);

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
                _membershipRepository.Add(purchaseResult.Membership);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(purchaseResult.Membership.Id);
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(MembershipErrors.Overlap);
        }
    }
}