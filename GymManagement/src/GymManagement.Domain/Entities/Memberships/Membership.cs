using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Invoices;
using GymManagement.Domain.Entities.Memberships.Events;
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
        DateTime? endDate)
        : base(id)
    {
        MembershipType = membershipType;
        PriceAmount = priceAmount;
        IsActive = isActive;
        User = user ?? throw new ArgumentNullException(nameof(user));
        Gym = gym ?? throw new ArgumentNullException(nameof(gym));
        StartDate = startDate;
        EndDate = endDate;
    }

    public MembershipType MembershipType { get; private set; }

    public DateTime? StartDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    public decimal PriceAmount { get; private set; }

    public bool IsActive { get; private set; }

    public User User { get; private set; }

    public List<Invoice> Invoices { get; } = [];

    public Gym Gym { get; private set; }

    public static Membership Buy(
        Guid userId,
        MembershipType membershipType,
        decimal priceAmount,
        User user,
        Gym gym
    )
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (gym == null) throw new ArgumentNullException(nameof(gym));

        var existingMembership = user.GetActiveMembershipInGym(gym);

        Membership membership;

        if (existingMembership is not null)
        {
            existingMembership.Extend(membershipType);
            membership = existingMembership;
        }
        else
        {
            var startDate = DateTime.UtcNow;
            var endDate = CalculateEndDate(startDate, membershipType);

            membership = new Membership(
                userId,
                membershipType,
                priceAmount,
                false,
                user,
                gym,
                startDate,
                endDate);

            // Ассоциируем абонемент с пользователем и залом
            user.AddMembership(membership);
            gym.AddMembership(membership);
        }

        var invoice = Invoice.Create(
            Guid.NewGuid(),
            DateTime.UtcNow,
            priceAmount,
            PaymentStatus.Pending,
            null, // Дата оплаты будет установлена при оплате
            membership);

        membership.Invoices.Add(invoice);
        membership.RaiseDomainEvent(new MembershipPurchasedDomainEvent(membership.Id));
        return membership;
    }

    public void Deactivate()
    {
        IsActive = false;

        // Можно будет добавить соответствующее событие, например, MembershipDeactivatedDomainEvent.
    }

    public void Activate()
    {
        IsActive = true;
        RaiseDomainEvent(new MembershipActivatedDomainEvent(Id));
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

        RaiseDomainEvent(new MembershipPurchasedDomainEvent(Id));
    }

    private static DateTime CalculateEndDate(DateTime startDate, MembershipType membershipType)
    {
        return membershipType switch
        {
            MembershipType.Daily => startDate.AddDays(1),
            MembershipType.Monthly => startDate.AddMonths(1),
            MembershipType.Quarterly => startDate.AddMonths(3),
            MembershipType.Annual => startDate.AddYears(1),
            _ => throw new ArgumentOutOfRangeException(nameof(membershipType), "Invalid membership type")
        };
    }
}