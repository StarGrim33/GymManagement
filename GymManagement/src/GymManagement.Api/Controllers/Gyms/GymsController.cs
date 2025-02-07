using GymManagement.Application.Gyms.Create;
using GymManagement.Application.Gyms.Delete;
using GymManagement.Application.Gyms.Get;
using GymManagement.Application.Gyms.Get.GetAll;
using GymManagement.Application.Gyms.Get.GetGymByQueryOptions;
using GymManagement.Application.Gyms.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers.Gyms
{
    [ApiController]
    [Route("api/gyms")]
    public class GymsController(ISender sender) : Controller
    {
        [HttpGet("{gymId:guid}")]
        public async Task<IActionResult> GetGym(
            Guid gymId,
            CancellationToken cancellationToken)
        {
            var query = new GetGymQuery(gymId);

            var result = await sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }

        [HttpGet("by-options")]
        public async Task<IActionResult> GetGymByQueryOptions(
            [FromQuery] GetGymByQueryOptionsRequest queryOptionsRequest,
            CancellationToken cancellationToken)
        {
            var query = new GetGymByQueryOptionsQuery(
                queryOptionsRequest.GymId,
                queryOptionsRequest.DoIncludeAmenities,
                queryOptionsRequest.DoIncludeTrainers,
                queryOptionsRequest.DoIncludeEquipment,
                queryOptionsRequest.DoIncludeMemberships,
                queryOptionsRequest.DoIncludeTrainingSessions,
                queryOptionsRequest.IsAsNoTracking);

            var result = await sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllGyms(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10, 
            CancellationToken cancellationToken = default)
        {
            var query = new GetAllGymsQuery(pageNumber, pageSize);

            var result = await sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGym([FromBody]CreateGymRequest request, CancellationToken cancellationToken)
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

        [HttpDelete("{gymId:guid}")]
        public async Task<IActionResult> DeleteGym(Guid gymId, CancellationToken cancellationToken)
        {
            var command = new DeleteGymCommand(gymId);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateGym([FromBody] UpdateGymRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateGymCommand(request.GymId, request.Name, request.Description, request.Address, request.Schedule);

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
