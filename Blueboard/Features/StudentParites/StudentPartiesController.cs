using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.StudentParites.Commands;
using Blueboard.Features.StudentParites.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace Blueboard.Features.StudentParites;

[Authorize]
[EmailVerified]
[FeatureGate("StudentParties")]
public class StudentPartiesController : ApiControllerBase
{
    [HttpGet("{id}")]
    [Permissions(typeof(StudentPartiesPermissions.ViewStudentParty),
        typeof(StudentPartiesPermissions.ViewApprovedStudentParty))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointSummary("Get information about a student party")]
    public async Task<ActionResult<ViewStudentParty.Response>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewStudentParty.Query
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpPost]
    [Permissions(typeof(StudentPartiesPermissions.CreateStudentParty))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EndpointSummary("Create a new student party")]
    public async Task<ActionResult<CreateStudentParty.Response>> Create([FromBody] CreateStudentParty.RequestBody body)
    {
        var response = await Mediator.Send(new CreateStudentParty.Command
        {
            Body = body
        });

        return CreatedAtAction(nameof(View), new { id = response.Id }, response);
    }
}