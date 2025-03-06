using GymManagement.Application.Abstractions.Email;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Memberships.Events;
using GymManagement.Domain.Entities.Users;
using MediatR;

namespace GymManagement.Application.Memberships.BuyMembership;

internal sealed class BuyMembershipDomainEventHandler
    : INotificationHandler<MembershipPurchasedDomainEvent>
{
    private readonly IEmailService _emailService;
    private readonly IMembershipRepository _membershipRepository;
    private readonly IUserRepository _userRepository;

    public BuyMembershipDomainEventHandler(
        IMembershipRepository membershipRepository,
        IUserRepository userRepository,
        IEmailService emailService)
    {
        _membershipRepository = membershipRepository;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task Handle(MembershipPurchasedDomainEvent notification, CancellationToken cancellationToken)
    {
        const string subjectMessage = "The subscription has been issued!";
        const string bodyMessage = "You have 10 minutes to confirm and pay this membership";

        var membership = await _membershipRepository.GetByIdAsync(notification.MembershipId, cancellationToken);

        if (membership is null) return;

        var user = await _userRepository.GetByIdAsync(notification.MembershipId, cancellationToken);

        if (user is null) return;

        await _emailService.SendAsync(
            user.Email.Value,
            subjectMessage,
            bodyMessage);
    }
}