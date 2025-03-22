using GymManagement.Application.Loging;
using GymManagement.Application.Users.CreateUser;
using GymManagement.Application.Users.GetLoggedInUser;
using GymManagement.Application.Users.GetUser;
using GymManagement.Application.Users.GetUser.GetAllUsers;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Endpoints.Users;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("users/{userId:guid}", GetUser)
            .RequireAuthorization()
            .WithName(nameof(GetUser));

        builder.MapGet("users", GetAllUsers)
            .RequireAuthorization()
            .WithName(nameof(GetAllUsers));

        builder.MapPost("users/create", CreateUser)
            .AllowAnonymous()
            .WithName(nameof(CreateUser));

        builder.MapGet("me", GetLoggedInUser)
            .RequireAuthorization("users:read")
            .WithName(nameof(GetLoggedInUser));

        builder.MapPost("login", LogIn)
            .AllowAnonymous()
            .WithName(nameof(LogIn));

        return builder;
    }

    private static async Task<IResult> GetUser(
        ISender sender,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetUserQuery(userId);

        var result = await sender.Send(query, cancellationToken);

        return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
    }

    private static async Task<IResult> GetAllUsers(
        ISender sender,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllUsersQuery(pageNumber, pageSize);

        var result = await sender.Send(query, cancellationToken);

        return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
    }

    private static async Task<IResult> CreateUser(
        ISender sender,
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var role = new Role(request.RoleId, string.Empty);

        var command = new CreateUserCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            request.PhoneNumber,
            request.DateOfBirth,
            request.IsActive,
            role,
            new Address(request.Address.Street, request.Address.City, request.Address.ZipCode));

        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
    }

    private static async Task<IResult> GetLoggedInUser(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();

        var result = await sender.Send(query, cancellationToken);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> LogIn(
        ISender sender,
        LogInUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(request.Email, request.Password);

        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure ? Results.Unauthorized() : Results.Ok(result.Value);
    }
}