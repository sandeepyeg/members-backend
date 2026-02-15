using EnterpriseMembers.Application.Features.Members.Commands;
using EnterpriseMembers.Application.Features.Members.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseMembers.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policy = "MembersRead")]
    public async Task<IActionResult> GetMembers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] bool expiredOnly = false,
        [FromQuery] string sortBy = "expiryDate",
        [FromQuery] string sortDir = "asc")
    {
        if (pageSize > 100) pageSize = 100;

        var query = new GetMembersQuery
        {
            Page = page,
            PageSize = pageSize,
            Search = search,
            ExpiredOnly = expiredOnly,
            SortBy = sortBy,
            SortDir = sortDir
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "MembersRead")]
    public async Task<IActionResult> GetMember(int id)
    {
        var query = new GetMemberByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new { message = $"Member with ID {id} not found" });
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "MembersWrite")]
    public async Task<IActionResult> CreateMember([FromBody] CreateMemberCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetMember), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "MembersWrite")]
    public async Task<IActionResult> UpdateMember(int id, [FromBody] UpdateMemberCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(new { message = "ID mismatch" });
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "MembersDelete")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        var command = new DeleteMemberCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
