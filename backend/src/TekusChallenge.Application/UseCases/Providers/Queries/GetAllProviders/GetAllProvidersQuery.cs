using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Providers.Queries.GetAllProviders;

public sealed record GetAllProvidersQuery : IRequest<ResponsePagination<IEnumerable<ProviderDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    
    public string? SearchTerm { get; init; }
    public string? Nit { get; init; }
    public string? Email { get; init; }
    
    public string? OrderBy { get; init; }
    public bool OrderDescending { get; init; } = false;
}
