using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Providers.Queries.GetProviderByNit;

public sealed record GetProviderByNitQuery : IRequest<Response<ProviderDto>>
{
    public string Nit { get; init; } = string.Empty;
}

