namespace GymManagement.Domain.Entities.Gyms;

public class GymDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public AddressDto Address { get; set; }

    public string Schedule { get; set; }
}