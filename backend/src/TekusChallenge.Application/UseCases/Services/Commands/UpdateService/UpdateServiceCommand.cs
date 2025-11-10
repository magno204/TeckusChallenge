using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Services.Commands.UpdateService
{
    public sealed record UpdateServiceCommand : IRequest<Response<ServiceDto>>
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal HourlyRate { get; init; }
        public string? Description { get; init; }
        public Guid ProviderId { get; init; }
        public List<string> CountryCodes { get; init; } = new();
    }
}

