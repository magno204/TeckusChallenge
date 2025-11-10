using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Providers.Commands.CreateProvider;

public sealed record CreateProviderCommand : IRequest<Response<ProviderDto>>
{
    public string Nit { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public List<CreateProviderCustomFieldDto>? CustomFields { get; init; }
}

public sealed record CreateProviderCustomFieldDto
{
    public string FieldName { get; init; } = string.Empty;
    public string FieldValue { get; init; } = string.Empty;
    public string FieldType { get; init; } = "text";
    public string? Description { get; init; }
    public int DisplayOrder { get; init; } = 0;
}
