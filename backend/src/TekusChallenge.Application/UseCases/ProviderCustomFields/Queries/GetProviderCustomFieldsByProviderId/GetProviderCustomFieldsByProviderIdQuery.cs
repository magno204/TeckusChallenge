using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.ProviderCustomFields.Queries.GetProviderCustomFieldsByProviderId;

/// <summary>
/// Query to get all custom fields for a specific provider
/// </summary>
public sealed record GetProviderCustomFieldsByProviderIdQuery : IRequest<Response<IEnumerable<ProviderCustomFieldDto>>>
{
    /// <summary>
    /// Provider ID to get custom fields for
    /// </summary>
    public Guid ProviderId { get; init; }
}

