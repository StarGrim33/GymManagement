using Dapper;
using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Memberships.MembershipTypes.Errors;

namespace GymManagement.Application.MembershipTypes.GetMembershipTypes;

internal sealed class GetMembershipTypesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IQueryHandler<GetMembershipTypesQuery, IReadOnlyList<MembershipTypesResponse>>
{
    public async Task<Result<IReadOnlyList<MembershipTypesResponse>>> Handle(GetMembershipTypesQuery request, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT *
                           FROM membership_type
                           """;

        using var connection = sqlConnectionFactory.CreateConnection();

        var membershipType = await connection
            .QueryAsync<MembershipTypesResponse>(
                sql);

        var membershipTypesResponses = membershipType.ToList();

        return membershipTypesResponses.Count == 0 
            ? Result.Failure<IReadOnlyList<MembershipTypesResponse>>(MembershipTypesErrors.NotFound) 
            : membershipTypesResponses.ToList();
    }
}