using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.ProviderCustomFields.Queries.GetProviderCustomFieldById;

/// <summary>
/// Query to get a specific custom field by its ID
/// </summary>
public sealed record GetProviderCustomFieldByIdQuery : IRequest<Response<ProviderCustomFieldDto>>
{
    /// <summary>
    /// Custom field ID to retrieve
    /// </summary>
    public Guid Id { get; init; }
}

