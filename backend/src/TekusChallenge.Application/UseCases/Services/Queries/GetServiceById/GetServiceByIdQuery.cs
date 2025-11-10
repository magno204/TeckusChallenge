using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Services.Queries.GetServiceById;

public sealed record GetServiceByIdQuery : IRequest<Response<ServiceDto>>
{
    public Guid Id { get; init; }
}

