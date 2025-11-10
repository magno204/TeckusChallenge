using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Providers.Queries.GetProviderById;

public sealed record GetProviderQuery : IRequest<Response<ProviderDto>>
{
    public Guid Id { get; init; }
}
