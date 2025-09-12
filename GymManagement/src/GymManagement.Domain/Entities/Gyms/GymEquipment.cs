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

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public EquipmentType EquipmentType { get; private set; }
    public bool IsAvailable { get; private set; }
    public Gym Gym { get; private set; } = null!;
    public Guid GymId { get; private set; }
}