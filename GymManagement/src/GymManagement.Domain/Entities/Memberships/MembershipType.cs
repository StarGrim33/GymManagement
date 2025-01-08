using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Memberships;

public class MembershipType : Entity
{
    private MembershipType(
        Guid id, 
        TimeSpan duration, 
        decimal price) 
        : base(id)
    {
        Duration = duration;
        Price = price;
    }
    
    public string Name { get; private set; }

    public TimeSpan Duration { get; private set; }

    public decimal Price { get; private set; }
}