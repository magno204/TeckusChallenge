using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Services.Queries.GetServicesByProviderId;

public sealed record GetServicesByProviderIdQuery : IRequest<Response<IEnumerable<ServiceDto>>>
{
    public Guid ProviderId { get; init; }
}

