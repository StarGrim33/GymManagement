namespace GymManagement.Api.Controllers.MembershipTypes;

public record CreateMembershipTypeRequest(string Name, int DurationInDays, decimal Price);