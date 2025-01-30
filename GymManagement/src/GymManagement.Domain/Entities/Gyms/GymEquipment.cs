using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Gyms;

public sealed class GymEquipment : Entity
{
    private GymEquipment(Guid id) 
        : base(id)
    {
    }

    private GymEquipment()
    {
    }

    public Name Name { get; private set; }

    public Description Description { get; private set; }

    public EquipmentType EquipmentType { get; private set; }

    public bool IsAvailable { get; private set; }

    public Gym Gym { get; private set; }

    public Guid GymId { get; private set; }
}