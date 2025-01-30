using GymManagement.Application.Gyms.CreateGym;
using GymManagement.Application.Gyms.GetGym;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers.Gyms
{
    [ApiController]
    [Route("api/gyms")]
    public class GymsController(ISender sender) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetGym(
            Guid gymId,
            CancellationToken cancellationToken)
        {
            var query = new GetGymQuery(gymId);

            var result = await sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGym(CreateGymRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateGymCommand(request.Name, request.Description, request.Address, request.Schedule);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetGym),
                new { id = result.Value }, result.Value);
        }
    }
}
