using Asp.Versioning;
using GymManagement.Application.Loging;
using GymManagement.Application.Users.CreateUser;
using GymManagement.Application.Users.GetLoggedInUser;
using GymManagement.Application.Users.GetUser;
using GymManagement.Application.Users.GetUser.GetAllUsers;
using GymManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers.Users
{
    [Authorize]
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/users")]
    public class UserController(ISender sender) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetUser(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var query = new GetUserQuery(userId);

            var result = await sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("all-users")]

        public async Task<IActionResult> GetAllUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        {
            var query = new GetAllUsersQuery(pageNumber, pageSize);

            var result = await sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateUserCommand(
                request.FirstName, 
                request.LastName, 
                request.Email,
                request.Password,
                request.PhoneNumber,
                request.DateOfBirth, 
                request.IsActive,
                request.Role,
                new Address(request.Address.Street, request.Address.City, request.Address.ZipCode));

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetUser),
                new { id = result.Value }, result.Value);
        }

        [HttpGet("me")]
        [Authorize(Roles = RolesConstants.Registered)]
        [Authorize(Policy = "users:read")]
        public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
        {
            var query = new GetLoggedInUserQuery();

            var result = await sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(
            LogInUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new LogInUserCommand(request.Email, request.Password);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }

            return Ok(result.Value);
        }
    }
}