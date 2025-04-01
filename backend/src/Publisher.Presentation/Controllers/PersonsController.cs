using MediatR;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.Persons.Queries.GetPersons;
using Microsoft.AspNetCore.Authorization;
using Publisher.Presentation.Authorization;

namespace Publisher.Presentation.Controllers;

[JwtAdmin]
[ApiController]
public class PersonsController(ISender _sender) : ControllerBase
{
    [HttpGet(ApiEndpoints.V1.Persons.GetAll)]
    public async Task<IActionResult> GetPersons(CancellationToken token)
    {
        return Ok(await _sender.Send(new GetPersonsQuery(), token));
    }
}
