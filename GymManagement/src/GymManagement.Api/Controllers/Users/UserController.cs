using GymManagement.Application.Users.CreateUser;
using GymManagement.Application.Users.GetUser;
using GymManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers.Users
{
    [ApiController]
    [Route("api/users")]
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

        [HttpPost]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateUserCommand(request.FirstName, 
                request.LastName, 
                request.Email,
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
    }
}