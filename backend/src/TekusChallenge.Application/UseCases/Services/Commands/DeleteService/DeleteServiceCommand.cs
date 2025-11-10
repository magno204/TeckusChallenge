using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekus.Transversal;

namespace TekusChallenge.Application.UseCases.Services.Commands.DeleteService
{
    public sealed record DeleteServiceCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; init; }
    }
}

