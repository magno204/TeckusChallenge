using MediatR;
using Tekus.Transversal;

namespace TekusChallenge.Application.UseCases.Auth.Commands.Login;

public sealed record LoginCommand : IRequest<Response<LoginResult>>
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public sealed record LoginResult
{
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public string Username { get; init; } = string.Empty;
}