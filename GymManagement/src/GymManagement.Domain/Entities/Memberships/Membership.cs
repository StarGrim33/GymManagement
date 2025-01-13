using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Invoices;
using GymManagement.Domain.Entities.Memberships.Errors;
using GymManagement.Domain.Entities.Memberships.Events;
using GymManagement.Domain.Entities.Memberships.MembershipTypes;
using GymManagement.Domain.Entities.Users;

namespace GymManagement.Domain.Entities.Memberships;

public sealed class Membership : Entity
{
    private Membership(
        Guid id,
        MembershipType membershipType,
        decimal priceAmount,
        bool isActive,
        User user,
        Gym gym,
        DateTime? startDate,
        DateTime? endDate,
        MembershipStatus membershipStatus)
        : base(id)
    {
        MembershipType = membershipType ?? throw new ArgumentNullException(nameof(membershipType));
        MembershipTypeId = membershipType.Id;
        PriceAmount = priceAmount;
        IsActive = isActive;
        User = user ?? throw new ArgumentNullException(nameof(user));
        UserId = user.Id;
        Gym = gym ?? throw new ArgumentNullException(nameof(gym));
        GymId = gym.Id;
        StartDate = startDate;
        EndDate = endDate;
        MembershipStatus = membershipStatus;
    }

    public MembershipType MembershipType { get; private set; }

    public Guid MembershipTypeId { get; private set; }

    public MembershipStatus MembershipStatus { get; private set; }

    public DateTime? StartDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    public decimal PriceAmount { get; private set; }

    public bool IsActive { get; private set; }

    public User User { get; private set; }

    public Guid UserId { get; private set; }

    public List<Invoice> Invoices { get; } = [];

    public Gym Gym { get; private set; }

    public Guid GymId { get; private set; }

    public static MembershipPurchaseResult Buy(
        MembershipType membershipType,
        User user,
        Gym gym
    )
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(gym);
        ArgumentNullException.ThrowIfNull(membershipType);

        var existingMembership = user.GetActiveMembershipInGym(gym);

        Membership membership;
        bool isNewMembership;

        if (existingMembership is not null)
        {
            existingMembership.Extend(membershipType);
            membership = existingMembership;
            isNewMembership = false;
        }
        else
        {
            membership = BuyNewMembership(membershipType, user, gym, out isNewMembership);
        }

        var invoice = Invoice.Create(
            Guid.NewGuid(),
            DateTime.UtcNow,
            membershipType.Price,
            PaymentStatus.Pending,
            null, // Дата оплаты будет установлена при оплате
            membership);

        membership.Invoices.Add(invoice);
        membership.MembershipStatus = MembershipStatus.Bought;
        return new MembershipPurchaseResult(membership, isNewMembership);
    }

    private static Membership BuyNewMembership(MembershipType membershipType, User user, Gym gym, out bool isNewMembership)
    {
        var startDate = DateTime.UtcNow;
        var endDate = CalculateEndDate(startDate, membershipType);

        var membership = new Membership(
            Guid.NewGuid(),
            membershipType,
            membershipType.Price,
            false,
            user,
            gym,
            startDate,
            endDate,
            MembershipStatus.Bought);

        // Ассоциируем абонемент с пользователем и залом
        user.AddMembership(membership);
        gym.AddMembership(membership);

        membership.RaiseDomainEvent(new MembershipPurchasedDomainEvent(membership.Id));
        isNewMembership = true;
        return membership;
    }

    public Result Freeze()
    {
        if (MembershipStatus is not MembershipStatus.Paid) 
            return Result.Failure(MembershipErrors.NotPaid);

        IsActive = false;
        MembershipStatus = MembershipStatus.Frozen;

        // Можно будет добавить соответствующее событие, например, MembershipDeactivatedDomainEvent.

        RaiseDomainEvent(new MembershipFrozenDomainEvent(Id));

        return Result.Success();
    }

    public Result MarkAsPaid()
    {
        if (MembershipStatus is not MembershipStatus.Bought)
            return Result.Failure(MembershipErrors.NotActivated);

        IsActive = true;
        MembershipStatus = MembershipStatus.Paid;
        RaiseDomainEvent(new MembershipPaidDomainEvent(Id));

        return Result.Success();
    }

    public Result UnFreeze()
    {
        if (MembershipStatus is not MembershipStatus.Frozen)
            return Result.Failure(MembershipErrors.NotActivated);

        IsActive = true;
        MembershipStatus = MembershipStatus.Active;
        RaiseDomainEvent(new MembershipUnfrozenDomainEvent(Id));

        return Result.Success();
    }

    private void Extend(MembershipType membershipType)
    {
        if (EndDate.HasValue && EndDate.Value > DateTime.UtcNow)
        {
            // Продлеваем от текущей даты окончания
            EndDate = CalculateEndDate(EndDate.Value, membershipType);
        }
        else
        {
            // Начинаем с текущей даты
            StartDate = DateTime.UtcNow;
            EndDate = CalculateEndDate(StartDate.Value, membershipType);
        }

        MembershipType = membershipType;
        MembershipTypeId = membershipType.Id;
        RaiseDomainEvent(new MembershipExtendedDomainEvent(Id));
    }

    private static DateTime CalculateEndDate(DateTime startDate, MembershipType membershipType)
    {
        if (membershipType.Duration == TimeSpan.Zero)
        {
            throw new ArgumentException("Membership duration must be greater than zero.", nameof(membershipType));
        }

        return startDate.Add(membershipType.Duration);
    }
}