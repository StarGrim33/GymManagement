using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Memberships.MembershipTypes;
using GymManagement.Domain.Entities.Memberships.MembershipTypes.Errors;

namespace GymManagement.Application.MembershipTypes.CreateMembershipType;

internal sealed class CreateMembershipTypeCommandHandler(
    IMembershipTypeRepository membershipTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateMembershipTypeCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateMembershipTypeCommand request, CancellationToken cancellationToken)
    {
        var existingMembershipType = await membershipTypeRepository.GetByNameAsync(request.Name.ToLower(), cancellationToken);

        if (existingMembershipType != null)
        {
            return Result.Failure<Guid>(MembershipTypesErrors.Exists);
        }

        var newMembershipType = MembershipType.Create(request.Name, request.Duration, request.Price);

        await membershipTypeRepository.AddAsync(newMembershipType, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(newMembershipType.Id);
    }
}