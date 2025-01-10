using Dapper;
using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Memberships.MembershipTypes.Errors;

namespace GymManagement.Application.MembershipTypes.GetMembershipTypes;

internal sealed class GetMembershipTypesQueryHandler : IQueryHandler<GetMembershipTypesQuery, IReadOnlyList<MembershipTypesResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetMembershipTypesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<MembershipTypesResponse>>> Handle(GetMembershipTypesQuery request, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT *
                               m.id AS Id,
                               m.name AS Name,
                               m.duration as Duration,
                               m.price as Price
                           FROM membership_types AS m
                           """;

        using var connection = _sqlConnectionFactory.CreateConnection();

        var membershipType = await connection
            .QueryAsync<MembershipTypesResponse>(
                sql);

        var membershipTypesResponses = membershipType.ToList();

        return membershipTypesResponses.Count == 0 
            ? Result.Failure<IReadOnlyList<MembershipTypesResponse>>(MembershipTypesErrors.NotFound) 
            : membershipTypesResponses.ToList();
    }
}