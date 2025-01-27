using GymManagement.Api.Controllers.MembershipTypes;
using GymManagement.Application.Memberships.BuyMembership;
using GymManagement.Application.Memberships.GetMembership;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers.Memberships
{
    [ApiController]
    [Route("api/memberships")]
    public class MembershipsController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetMembership(
            Guid membershipId, 
            CancellationToken cancellationToken)
        {
            var query = new GetMembershipQuery(membershipId);

            var result = await sender.Send(query, cancellationToken);

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
    }
}
