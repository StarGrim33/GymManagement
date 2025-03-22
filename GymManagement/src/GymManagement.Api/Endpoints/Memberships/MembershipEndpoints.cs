using GymManagement.Application.Memberships.BuyMembership;
using GymManagement.Application.Memberships.GetMembership;
using GymManagement.Application.Memberships.GetMembership.GetAllMemberships;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Endpoints.Memberships;

public static class MembershipEndpoints
{
    public static IEndpointRouteBuilder MapMembershipEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("memberships/{membershipId:guid}", GetMembership)
            .RequireAuthorization()
            .WithName(nameof(GetMembership));

        builder.MapPost("memberships/buy", BuyMembership)
            .RequireAuthorization()
            .WithName(nameof(BuyMembership));

        builder.MapGet("memberships", GetAllMemberships)
            .RequireAuthorization()
            .WithName(nameof(GetAllMemberships));

        return builder;
    }

    private static async Task<IResult> GetMembership(
        ISender sender,
        Guid membershipId,
        CancellationToken cancellationToken)
    {
        var query = new GetMembershipQuery(membershipId);

        var result = await sender.Send(query, cancellationToken);

        return result.IsFailure
            ? Results.BadRequest(result.Error)
            : Results.Ok(result.Value);
    }

    private static async Task<IResult> BuyMembership(
        ISender sender,
        [FromBody] BuyMembershipRequest request,
        CancellationToken cancellationToken)
    {
        var command = new BuyMembershipCommand(
            request.UserId,
            request.MembershipTypeId,
            request.GymId);

        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure
            ? Results.BadRequest(result.Error)
            : Results.CreatedAtRoute(
                nameof(GetMembership),
                new { membershipId = result.Value },
                result.Value);
    }

    private static async Task<IResult> GetAllMemberships(
        ISender sender,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllMembershipsQuery(pageNumber, pageSize);

        var result = await sender.Send(query, cancellationToken);

        return result.IsFailure
            ? Results.BadRequest(result.Error)
            : Results.Ok(result.Value);
    }
}