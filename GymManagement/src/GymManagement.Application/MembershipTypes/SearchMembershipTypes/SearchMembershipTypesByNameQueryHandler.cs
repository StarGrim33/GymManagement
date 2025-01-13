using Dapper;
using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Memberships.MembershipTypes.Errors;

namespace GymManagement.Application.MembershipTypes.SearchMembershipTypes;

internal sealed class SearchMembershipTypesByNameQueryHandler
    : IQueryHandler<SearchMembershipTypesByNameQuery, MembershipTypesResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public SearchMembershipTypesByNameQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

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

        using var connection = _sqlConnectionFactory.CreateConnection();

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