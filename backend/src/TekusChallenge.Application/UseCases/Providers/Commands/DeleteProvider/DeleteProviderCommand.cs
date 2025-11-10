using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekus.Transversal;

namespace TekusChallenge.Application.UseCases.Providers.Commands.DeleteProvider
{
    public sealed record DeleteProviderCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; init; }
    }
}

