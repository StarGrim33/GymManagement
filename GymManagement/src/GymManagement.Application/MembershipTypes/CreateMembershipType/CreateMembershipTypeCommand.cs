using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.MembershipTypes.CreateMembershipType;

public sealed record CreateMembershipTypeCommand(string Name, 
    TimeSpan Duration, 
    decimal Price) : ICommand<Guid>;