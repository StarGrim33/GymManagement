using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities;

public sealed class GymEquipment(Guid id) : Entity(id)
{
    public Name Name { get; private set; }

    public Description Description { get; private set; }

    public EquipmentType EquipmentType { get; private set; }

    public bool IsAvailable { get; private set; }

    public Gym Gym { get; private set; }
}