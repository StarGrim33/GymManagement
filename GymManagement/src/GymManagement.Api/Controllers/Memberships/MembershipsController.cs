using GymManagement.Application.Memberships.BuyMembership;
using GymManagement.Application.Memberships.GetMembership;
using GymManagement.Application.Memberships.GetMembership.GetAllMemberships;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers.Memberships
{
    [ApiController]
    [Route("api/memberships")]
    public class MembershipsController(ISender sender) : ControllerBase
    {
        [HttpGet("{gymId:guid}")]
        public async Task<IActionResult> GetMembership(
            Guid membershipId, 
            CancellationToken cancellationToken)
        {
            var query = new GetMembershipQuery(membershipId);

            var result = await sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> BuyMembership(
            BuyMembershipRequest request,
            CancellationToken cancellationToken)
        {
            var command = new BuyMembershipCommand(
                request.UserId,
                request.MembershipTypeId,
                request.GymId);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetMembership), 
                new { id = result.Value }, result.Value);
        }

        [HttpGet("all-memberships")]
        public async Task<IActionResult> GetAllMemberships(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = new GetAllMembershipsQuery(pageNumber, pageSize);

            var result = await sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
