using GymManagement.Application.Gyms.Create;
using GymManagement.Application.Gyms.Delete;
using GymManagement.Application.Gyms.Get;
using GymManagement.Application.Gyms.Get.GetAll;
using GymManagement.Application.Gyms.Get.GetGymByQueryOptions;
using GymManagement.Application.Gyms.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Endpoints.Gyms;

public static class GymEndpoints
{
    public static IEndpointRouteBuilder MapGymEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("gyms/{gymId:guid}", GetGym)
            .RequireAuthorization()
            .WithName(nameof(GetGym));

        builder.MapGet("gyms/by-options", GetGymByQueryOptions)
            .RequireAuthorization()
            .WithName(nameof(GetGymByQueryOptions));

        builder.MapGet("gyms", GetAllGyms)
            .RequireAuthorization()
            .WithName(nameof(GetAllGyms));

        builder.MapPost("gyms", CreateGym)
            .RequireAuthorization()
            .WithName(nameof(CreateGym));

        builder.MapDelete("gyms/{gymId:guid}", DeleteGym)
            .RequireAuthorization()
            .WithName(nameof(DeleteGym));

        builder.MapPut("gyms/{gymId:guid}", UpdateGym)
            .RequireAuthorization()
            .WithName(nameof(UpdateGym));

        return builder;
    }

    private static async Task<IResult> GetGym(
        ISender sender,
        Guid gymId,
        CancellationToken cancellationToken)
    {
        var query = new GetGymQuery(gymId);

        var result = await sender.Send(query, cancellationToken);

        return result.IsFailure
            ? Results.BadRequest(result.Error)
            : Results.Ok(result.Value);
    }

    private static async Task<IResult> GetGymByQueryOptions(
        ISender sender,
        [AsParameters] GetGymByQueryOptionsRequest queryOptionsRequest,
        CancellationToken cancellationToken)
    {
        var query = new GetGymByQueryOptionsQuery(
            queryOptionsRequest.GymId,
            queryOptionsRequest.IsAsNoTracking);

        var result = await sender.Send(query, cancellationToken);

        return result.IsFailure
            ? Results.BadRequest(result.Error)
            : Results.Ok(result.Value);
    }

    private static async Task<IResult> GetAllGyms(
        ISender sender,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllGymsQuery(pageNumber, pageSize);

        var result = await sender.Send(query, cancellationToken);

        return result.IsFailure
            ? Results.BadRequest(result.Error)
            : Results.Ok(result.Value);
    }

    private static async Task<IResult> CreateGym(
        ISender sender,
        [FromBody] CreateGymRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateGymCommand(
            request.Name,
            request.Description,
            request.Address,
            request.Schedule);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.CreatedAtRoute(
            nameof(GetGym),
            new { gymId = result.Value },
            result.Value);
    }

    private static async Task<IResult> DeleteGym(
        ISender sender,
        Guid gymId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteGymCommand(gymId);

        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
    }

    private static async Task<IResult> UpdateGym(
        ISender sender,
        Guid gymId,
        [FromBody] UpdateGymRequest request,
        CancellationToken cancellationToken)
    {
        if (gymId != request.GymId)
        {
            return Results.BadRequest("Идентификатор зала в маршруте и теле запроса не совпадают.");
        }

        var command = new UpdateGymCommand(
            request.GymId,
            request.Name,
            request.Description,
            request.Address,
            request.Schedule);

        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
    }
}
