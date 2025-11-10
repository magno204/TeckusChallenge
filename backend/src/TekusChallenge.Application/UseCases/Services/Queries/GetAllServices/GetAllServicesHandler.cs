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

namespace TekusChallenge.Application.UseCases.Services.Queries.GetAllServices;

public class GetAllServicesHandler : IRequestHandler<GetAllServicesQuery, ResponsePagination<IEnumerable<ServiceDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllServicesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponsePagination<IEnumerable<ServiceDto>>> Handle(
        GetAllServicesQuery request, 
        CancellationToken cancellationToken)
    {
        var response = new ResponsePagination<IEnumerable<ServiceDto>>();

        try
        {
            Expression<Func<Service, bool>>? filter = BuildFilter(request);

            Func<IQueryable<Service>, IOrderedQueryable<Service>>? orderBy = BuildOrderBy(request);

            var (items, totalCount) = await _unitOfWork.Services.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                filter,
                orderBy,
                cancellationToken);

            var serviceDtos = _mapper.Map<IEnumerable<ServiceDto>>(items);

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            response.Data = serviceDtos;
            response.IsSuccess = true;
            response.Message = "Services retrieved successfully.";
            response.PageNumber = request.PageNumber;
            response.TotalPages = totalPages;
            response.TotalCount = totalCount;

            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error retrieving services: {ex.Message}";
            return response;
        }
    }

    private Expression<Func<Service, bool>>? BuildFilter(GetAllServicesQuery request)
    {
        Expression<Func<Service, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            filter = s => s.Name.ToLower().Contains(searchTerm) ||
                         (s.Description != null && s.Description.ToLower().Contains(searchTerm));
        }

        if (request.ProviderId.HasValue && request.ProviderId.Value != Guid.Empty)
        {
            var providerFilter = (Expression<Func<Service, bool>>)(s => s.ProviderId == request.ProviderId.Value);
            filter = filter == null ? providerFilter : CombineFilters(filter, providerFilter);
        }

        return filter;
    }

    private Expression<Func<Service, bool>> CombineFilters(
        Expression<Func<Service, bool>> first,
        Expression<Func<Service, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(Service));
        var combined = Expression.AndAlso(
            Expression.Invoke(first, parameter),
            Expression.Invoke(second, parameter));
        return Expression.Lambda<Func<Service, bool>>(combined, parameter);
    }

    private Func<IQueryable<Service>, IOrderedQueryable<Service>>? BuildOrderBy(GetAllServicesQuery request)
    {
        if (string.IsNullOrWhiteSpace(request.OrderBy))
        {
            return query => query.OrderByDescending(s => s.CreatedAt);
        }

        return request.OrderBy.ToLower() switch
        {
            "name" => request.OrderDescending
                ? query => query.OrderByDescending(s => s.Name)
                : query => query.OrderBy(s => s.Name),

            "hourlyrate" => request.OrderDescending
                ? query => query.OrderByDescending(s => s.HourlyRate)
                : query => query.OrderBy(s => s.HourlyRate),

            "createdat" => request.OrderDescending
                ? query => query.OrderByDescending(s => s.CreatedAt)
                : query => query.OrderBy(s => s.CreatedAt),

            "updatedat" => request.OrderDescending
                ? query => query.OrderByDescending(s => s.UpdatedAt)
                : query => query.OrderBy(s => s.UpdatedAt),

            _ => query => query.OrderByDescending(s => s.CreatedAt)
        };
    }
}

