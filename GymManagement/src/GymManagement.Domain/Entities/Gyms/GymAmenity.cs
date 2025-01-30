using GymManagement.Domain.Entities.Invoices;

namespace GymManagement.Domain.Entities.Gyms;

public sealed class GymAmenity
{
    private GymAmenity(Guid gymId, Gym gym, Amenity amenity)
    {
        GymId = gymId;
        Gym = gym;
        Amenity = amenity;
    }

    private GymAmenity()
    {
    }

    public Guid GymId { get; set; }

    public Gym Gym { get; set; }

    public Amenity Amenity { get; set; }
}