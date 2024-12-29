using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities;

public sealed class Membership(Guid id) : Entity(id)
{
    public MembershipType MembershipType { get; private set; }

    public DateTime? StartDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    public decimal PriceAmount { get; private set; }

    public bool IsActive { get; private set; }

    public User User { get; private set; }

    public List<Invoice> Invoices { get; private set; } = [];

    public Gym Gym { get; private set; }
}