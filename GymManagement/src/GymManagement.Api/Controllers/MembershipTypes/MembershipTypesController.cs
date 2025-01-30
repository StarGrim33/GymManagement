using GymManagement.Application.MembershipTypes.GetMembershipTypes;
using GymManagement.Application.MembershipTypes.SearchMembershipTypes;
using GymManagement.Domain.Entities.Memberships.MembershipTypes.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers.MembershipTypes
{
    [ApiController]
    [Route("api/membershipTypes")]
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
    }
}
