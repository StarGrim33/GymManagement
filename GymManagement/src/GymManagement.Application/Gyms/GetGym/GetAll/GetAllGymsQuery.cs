using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Application.Common;

namespace GymManagement.Application.Gyms.GetGym.GetAll;

public record GetAllGymsQuery(int PageNumber, int PageSize) : IQuery<PaginatedList<GymResponse>>;