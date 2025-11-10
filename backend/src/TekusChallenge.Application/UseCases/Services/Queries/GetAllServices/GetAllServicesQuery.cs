using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Services.Queries.GetAllServices;

public sealed record GetAllServicesQuery : IRequest<ResponsePagination<IEnumerable<ServiceDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    
    public string? SearchTerm { get; init; }
    public Guid? ProviderId { get; init; }
    
    public string? OrderBy { get; init; }
    public bool OrderDescending { get; init; } = false;
}

