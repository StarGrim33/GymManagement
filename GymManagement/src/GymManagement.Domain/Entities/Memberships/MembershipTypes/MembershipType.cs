using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Memberships.MembershipTypes;

public class MembershipType : Entity
{
    private MembershipType(
        Guid id,
        string name,
        TimeSpan duration,
        decimal price)
        : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Duration = duration;
        Price = price;
    }

    private MembershipType()
    {
    }


    public string Name { get; private set; }

    public TimeSpan Duration { get; private set; }

    public decimal Price { get; private set; }

    public List<Membership> Memberships { get; private set; } = [];

    public static MembershipType Create(string name,
        TimeSpan duration,
        decimal price)
    {
        ArgumentNullException.ThrowIfNull(name);

        ArgumentNullException.ThrowIfNull(duration);

        ArgumentNullException.ThrowIfNull(price);

        var membershipType = new MembershipType(Guid.NewGuid(), name, duration, price);

        return membershipType;
    }
}