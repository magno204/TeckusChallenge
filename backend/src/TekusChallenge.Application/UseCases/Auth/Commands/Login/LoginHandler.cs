using MediatR;
using Tekus.Transversal;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Application.UseCases.Auth.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, Response<LoginResult>>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAuthenticationSettings _authSettings;

    public LoginHandler(IJwtTokenService jwtTokenService, IAuthenticationSettings authSettings)
    {
        _jwtTokenService = jwtTokenService;
        _authSettings = authSettings;
    }

    public async Task<Response<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<LoginResult>();

        var validUsername = _authSettings.GetValidUsername();
        var validPassword = _authSettings.GetValidPassword();

        if (request.Username != validUsername || request.Password != validPassword)
        {
            response.IsSuccess = false;
            response.Message = "Invalid username or password";
            return response;    
        }

        var token = _jwtTokenService.GenerateToken(request.Username);
        var expiresAt = _jwtTokenService.GetTokenExpiration();

        var result = new LoginResult
        {
            Token = token,
            ExpiresAt = expiresAt,
            Username = request.Username
        };

        response.IsSuccess = true;
        response.Message = "Authentication successful";
        response.Data = result;
        return response;
    }
}