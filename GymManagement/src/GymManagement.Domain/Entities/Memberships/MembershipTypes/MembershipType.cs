using GymManagement.Domain.Abstractions;
using System.Xml.Linq;

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


    public string Name { get; private set; }

    public TimeSpan Duration { get; private set; }

    public decimal Price { get; private set; }

    public List<Membership> Memberships { get; private set; } = [];
}