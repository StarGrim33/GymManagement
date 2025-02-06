using Dapper;
using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;

namespace GymManagement.Application.Memberships.GetMembership;

internal sealed class GetMembershipQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IQueryHandler<GetMembershipQuery, MembershipResponse>
{
    public async Task<Result<MembershipResponse>> Handle(GetMembershipQuery request,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT
                               id AS Id,
                               name AS Name,
                               description AS Description,
                               street AS Status,
                               price_amount AS PriceAmount,
                               start_date AS StartDate,
                               end_date AS EndDate,
                               membership_type AS MembershipType
                           FROM memberships
                           WHERE id = @MembershipId
                           """
        ;

        using var connection = sqlConnectionFactory.CreateConnection();

        var membership = await connection.QueryFirstOrDefaultAsync<MembershipResponse>(
            sql,
            new
            {
                request.MembershipId
            });

        return membership ?? new MembershipResponse();
    }
}