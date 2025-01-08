using Dapper;
using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;

namespace GymManagement.Application.MembershipTypes.SearchMembershipTypes;

internal sealed class SearchMembershipTypesQueryHandler
    : IQueryHandler<SearchMembershipTypesQuery, IReadOnlyList<MembershipTypesResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public SearchMembershipTypesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<MembershipTypesResponse>>> Handle(
        SearchMembershipTypesQuery request,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT
                               a.id AS Id,
                               a.name AS Name,
                               a.duration as Duration,
                               a.price as Price
                           FROM membership_types AS a
                           """;

        if (string.IsNullOrEmpty(request.Name))
        {
            return new List<MembershipTypesResponse>();
        }

        using var connection = _sqlConnectionFactory.CreateConnection();

        var apartments = await connection
            .QueryAsync<MembershipTypesResponse>(
                sql,
                new
                {
                    request.Name
                });

        return apartments.ToList();
    }
}