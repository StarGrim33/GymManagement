using GymManagement.Application.MembershipTypes.CreateMembershipType;
using GymManagement.Application.MembershipTypes.GetMembershipTypes;
using GymManagement.Application.MembershipTypes.SearchMembershipTypes;
using GymManagement.Domain.Entities.Memberships.MembershipTypes.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Endpoints.MembershipTypes;

public static class MembershipTypesEndpoints
{
    public static IEndpointRouteBuilder MapMembershipTypesEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("membershipTypes", GetMembershipTypes)
            .RequireAuthorization()
            .WithName(nameof(GetMembershipTypes));

        builder.MapGet("membershipTypes/{name}", GetMembershipTypesByName)
            .RequireAuthorization()
            .WithName(nameof(GetMembershipTypesByName));

        builder.MapPost("membershipTypes", CreateMembershipType)
            .RequireAuthorization()
            .WithName(nameof(CreateMembershipType));

        return builder;
    }

    private static async Task<IResult> GetMembershipTypes(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetMembershipTypesQuery();

        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
    }

    private static async Task<IResult> GetMembershipTypesByName(
        ISender sender,
        string name,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(name)) return Results.BadRequest(MembershipTypesErrors.EmptyName);

        var query = new SearchMembershipTypesByNameQuery(name);

        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
    }

    private static async Task<IResult> CreateMembershipType(
        ISender sender,
        [FromBody] CreateMembershipTypeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateMembershipTypeCommand(
            request.Name,
            TimeSpan.FromDays(request.DurationInDays),
            request.Price
        );

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.CreatedAtRoute(nameof(GetMembershipTypes), new { id = result.Value }) : Results.BadRequest(result.Error);
    }
}