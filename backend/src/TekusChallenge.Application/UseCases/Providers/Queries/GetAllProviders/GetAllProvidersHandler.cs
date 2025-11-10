using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Application.UseCases.Providers.Queries.GetAllProviders;

public class GetAllProvidersHandler : IRequestHandler<GetAllProvidersQuery, ResponsePagination<IEnumerable<ProviderDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllProvidersHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponsePagination<IEnumerable<ProviderDto>>> Handle(
        GetAllProvidersQuery request, 
        CancellationToken cancellationToken)
    {
        var response = new ResponsePagination<IEnumerable<ProviderDto>>();

        try
        {
            Expression<Func<Provider, bool>>? filter = BuildFilter(request);

            Func<IQueryable<Provider>, IOrderedQueryable<Provider>>? orderBy = BuildOrderBy(request);

            var (items, totalCount) = await _unitOfWork.Providers.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                filter,
                orderBy,
                cancellationToken);

            var providerDtos = _mapper.Map<IEnumerable<ProviderDto>>(items);

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            response.Data = providerDtos;
            response.IsSuccess = true;
            response.Message = "Providers retrieved successfully.";
            response.PageNumber = request.PageNumber;
            response.TotalPages = totalPages;
            response.TotalCount = totalCount;

            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error retrieving providers: {ex.Message}";
            return response;
        }
    }

    private Expression<Func<Provider, bool>>? BuildFilter(GetAllProvidersQuery request)
    {
        Expression<Func<Provider, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            filter = p => p.Name.ToLower().Contains(searchTerm) ||
                         p.Nit.ToLower().Contains(searchTerm) ||
                         p.Email.ToLower().Contains(searchTerm);
        }

        if (!string.IsNullOrWhiteSpace(request.Nit))
        {
            var nit = request.Nit.ToLower();
            var nitFilter = (Expression<Func<Provider, bool>>)(p => p.Nit.ToLower().Contains(nit));
            filter = filter == null ? nitFilter : CombineFilters(filter, nitFilter);
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var email = request.Email.ToLower();
            var emailFilter = (Expression<Func<Provider, bool>>)(p => p.Email.ToLower().Contains(email));
            filter = filter == null ? emailFilter : CombineFilters(filter, emailFilter);
        }

        return filter;
    }

    private Expression<Func<Provider, bool>> CombineFilters(
        Expression<Func<Provider, bool>> first,
        Expression<Func<Provider, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(Provider));
        var combined = Expression.AndAlso(
            Expression.Invoke(first, parameter),
            Expression.Invoke(second, parameter));
        return Expression.Lambda<Func<Provider, bool>>(combined, parameter);
    }

    private Func<IQueryable<Provider>, IOrderedQueryable<Provider>>? BuildOrderBy(GetAllProvidersQuery request)
    {
        if (string.IsNullOrWhiteSpace(request.OrderBy))
        {
            return query => query.OrderByDescending(p => p.CreatedAt);
        }

        return request.OrderBy.ToLower() switch
        {
            "name" => request.OrderDescending
                ? query => query.OrderByDescending(p => p.Name)
                : query => query.OrderBy(p => p.Name),

            "nit" => request.OrderDescending
                ? query => query.OrderByDescending(p => p.Nit)
                : query => query.OrderBy(p => p.Nit),

            "email" => request.OrderDescending
                ? query => query.OrderByDescending(p => p.Email)
                : query => query.OrderBy(p => p.Email),

            "createdat" => request.OrderDescending
                ? query => query.OrderByDescending(p => p.CreatedAt)
                : query => query.OrderBy(p => p.CreatedAt),

            "updatedat" => request.OrderDescending
                ? query => query.OrderByDescending(p => p.UpdatedAt)
                : query => query.OrderBy(p => p.UpdatedAt),

            _ => query => query.OrderByDescending(p => p.CreatedAt)
        };
    }
}
