namespace GymManagement.Api.Endpoints.MembershipTypes;

public record CreateMembershipTypeRequest(string Name, int DurationInDays, decimal Price);