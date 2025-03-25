using MediatR;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.Authentication.Commands.RefreshToken;
using Publisher.Application.Authentication.Commands.Register;
using Publisher.Application.Authentication.Queries.Login;
using Publisher.Contracts.Requests;
using Publisher.Contracts.Responses;

namespace Publisher.Presentation.Controllers;

[ApiController]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost(ApiEndpoints.V1.Auth.Register)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken token)
    {
        var command = new RegisterCommand(request.Username, request.Email, request.Password, request.Role);
        var authResult = await sender.Send(command, token);
        return Ok(new AuthenticationResponse(authResult.User.UserId, authResult.User.Username, authResult.User.Email, authResult.AccessToken, authResult.RefreshToken));
    }

    [HttpPost(ApiEndpoints.V1.Auth.Login)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken token)
    {
        var query = new LoginQuery(request.Email, request.Password);
        var authResult = await sender.Send(query, token);
        return Ok(new AuthenticationResponse(authResult.User.UserId, authResult.User.Username, authResult.User.Email, authResult.AccessToken, authResult.RefreshToken));
    }

    [HttpPost(ApiEndpoints.V1.Auth.RefreshToken)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken token)
    {
        var command = new RefreshTokenCommand(request.UserId, request.RefreshToken);
        var authResult = await sender.Send(command, token);
        return Ok(new AuthenticationResponse(authResult.User.UserId, authResult.User.Username, authResult.User.Email, authResult.AccessToken, authResult.RefreshToken));
    }
}
