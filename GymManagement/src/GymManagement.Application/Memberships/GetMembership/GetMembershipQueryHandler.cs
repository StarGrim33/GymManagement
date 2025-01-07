using Dapper;
using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;

namespace GymManagement.Application.Memberships.GetMembership;

internal sealed class GetMembershipQueryHandler : IQueryHandler<GetMembershipQuery, MembershipResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetMembershipQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<MembershipResponse>> Handle(GetMembershipQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = "string.Empty";

        var membership = await connection.QueryFirstOrDefaultAsync<MembershipResponse>(
            sql,
            new
            {
                request.MembershipId
            });

        return membership;
    }
}