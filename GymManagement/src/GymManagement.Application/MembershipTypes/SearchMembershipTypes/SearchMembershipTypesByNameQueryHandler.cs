using Dapper;
using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Memberships.MembershipTypes.Errors;

namespace GymManagement.Application.MembershipTypes.SearchMembershipTypes;

internal sealed class SearchMembershipTypesByNameQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IQueryHandler<SearchMembershipTypesByNameQuery, MembershipTypesResponse>
{
    public async Task<Result<MembershipTypesResponse>> Handle(
        SearchMembershipTypesByNameQuery request,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT
                               a.id AS Id,
                               a.name AS Name,
                               a.duration as Duration,
                               a.price as Price
                           FROM membership_types AS a
                           WHERE a.name = @Name
                           """;

        if (string.IsNullOrEmpty(request.Name))
        {
            Result.Failure<MembershipTypesResponse>(MembershipTypesErrors.EmptyName);
        }

        using var connection = sqlConnectionFactory.CreateConnection();

        var membershipType = await connection
            .QueryFirstOrDefaultAsync<MembershipTypesResponse>(
                sql,
                new
                { 
                    request.Name
                });

        return membershipType ?? Result.Failure<MembershipTypesResponse>(MembershipTypesErrors.NotFound);
    }
}