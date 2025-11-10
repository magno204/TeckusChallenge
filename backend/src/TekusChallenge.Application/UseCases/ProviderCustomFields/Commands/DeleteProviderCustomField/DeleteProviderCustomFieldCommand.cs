using MediatR;
using Tekus.Transversal;

namespace TekusChallenge.Application.UseCases.ProviderCustomFields.Commands.DeleteProviderCustomField;

public sealed record DeleteProviderCustomFieldCommand : IRequest<Response<bool>>
{
    public Guid Id { get; init; }
}

