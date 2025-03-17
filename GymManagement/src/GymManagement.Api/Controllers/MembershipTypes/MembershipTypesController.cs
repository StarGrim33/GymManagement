using Asp.Versioning;
using GymManagement.Application.MembershipTypes.CreateMembershipType;
using GymManagement.Application.MembershipTypes.GetMembershipTypes;
using GymManagement.Application.MembershipTypes.SearchMembershipTypes;
using GymManagement.Domain.Entities.Memberships.MembershipTypes.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers.MembershipTypes
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/membershipTypes")]
    public class MembershipTypesController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetMembershipTypes(CancellationToken cancellationToken)
        {
            var query = new GetMembershipTypesQuery();

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetMembershipTypesByName(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(MembershipTypesErrors.EmptyName);
            }

            var query = new SearchMembershipTypesByNameQuery(name);

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMembershipType([FromBody] CreateMembershipTypeRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateMembershipTypeCommand
            (
                request.Name,
                TimeSpan.FromDays(request.DurationInDays),
                request.Price
            );

            var result = await sender.Send(command, cancellationToken);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetMembershipTypes), new { id = result.Value }, null);
            }

            return BadRequest(result.Error);
        }
    }
}
