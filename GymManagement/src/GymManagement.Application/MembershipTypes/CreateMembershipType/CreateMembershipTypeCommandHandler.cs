using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Memberships.MembershipTypes;
using GymManagement.Domain.Entities.Memberships.MembershipTypes.Errors;

namespace GymManagement.Application.MembershipTypes.CreateMembershipType;

internal sealed class CreateMembershipTypeCommandHandler : ICommandHandler<CreateMembershipTypeCommand, Guid>
{
    private readonly IMembershipTypeRepository _membershipTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMembershipTypeCommandHandler(IMembershipTypeRepository membershipTypeRepository, IUnitOfWork unitOfWork)
    {
        _membershipTypeRepository = membershipTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateMembershipTypeCommand request, CancellationToken cancellationToken)
    {
        var existingMembershipType = await _membershipTypeRepository.GetByNameAsync(request.Name.ToLower(), cancellationToken);

        if (existingMembershipType != null)
        {
            return Result.Failure<Guid>(MembershipTypesErrors.Exists);
        }

        var newMembershipType = MembershipType.Create(request.Name, request.Duration, request.Price);

        _membershipTypeRepository.Add(newMembershipType);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(newMembershipType.Id);
    }
}