using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Providers.Commands.UpdateProvider;

public sealed record UpdateProviderCommand : IRequest<Response<ProviderDto>>
{
    public Guid Id { get; init; }
    public string Nit { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public List<UpdateProviderCustomFieldDto>? CustomFields { get; init; }
}

public sealed record UpdateProviderCustomFieldDto
{
    public Guid? Id { get; init; }
    public string FieldName { get; init; } = string.Empty;
    public string FieldValue { get; init; } = string.Empty;
    public string FieldType { get; init; } = "text";
    public string? Description { get; init; }
    public int DisplayOrder { get; init; } = 0;
}
