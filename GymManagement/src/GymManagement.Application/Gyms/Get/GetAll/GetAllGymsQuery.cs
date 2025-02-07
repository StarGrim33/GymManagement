using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Application.Common;
using GymManagement.Application.Gyms.Get;

namespace GymManagement.Application.Gyms.Get.GetAll;

public record GetAllGymsQuery(int PageNumber, int PageSize) : IQuery<PaginatedList<GymResponse>>;