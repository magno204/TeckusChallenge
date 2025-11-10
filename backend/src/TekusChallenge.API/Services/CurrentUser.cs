using System.Security.Claims;
using TekusChallenge.Application.Common.Constants;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.API.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue("username") ?? GlobalConstant.DefaultUserName;
}
