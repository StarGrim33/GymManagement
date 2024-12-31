using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Invoices;
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
        Gym gym, DateTime? startDate, DateTime? endDate) 
        : base(id)
    {
        MembershipType = membershipType;
        PriceAmount = priceAmount;
        IsActive = isActive;
        User = user;
        Gym = gym;
        StartDate = startDate;
        EndDate = endDate;
    }

    public MembershipType MembershipType { get; private set; }

    public DateTime? StartDate { get; }

    public DateTime? EndDate { get; }

    public decimal PriceAmount { get; private set; }

    public bool IsActive { get; private set; }

    public User User { get; private set; }

    private List<Invoice> Invoices { get; set; } = [];

    public Gym Gym { get; private set; }

    public static Membership Reserve(
        Guid userId,
        MembershipType membershipType,
        decimal priceAmount,
        User user,
        Gym gym
    )
    {
        if (user.HasActiveMembershipInGym(gym))
        {
            throw new InvalidOperationException("User already has an active membership in this gym.");
        }

        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddMonths(1);

        var membership = new Membership(
            userId,
            membershipType,
            priceAmount,
            isActive: true,
            user,
            gym,
            startDate,
            endDate);

        var invoice = Invoice.Create(
            Guid.NewGuid(),
            DateTime.UtcNow,
            priceAmount,
            PaymentStatus.Pending,
            null, // Дата оплаты будет установлена при оплате
            membership);

        membership.Invoices.Add(invoice);

        // Ассоциируем абонемент с пользователем и залом
        user.AddMembership(membership);
        gym.AddMembership(membership);

        return membership;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}