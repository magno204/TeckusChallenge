using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;
using TekusChallenge.Domain.Interfaces;
using AutoMapper;

namespace TekusChallenge.Application.UseCases.Services.Queries.GetServicesByProviderId;

public class GetServicesByProviderIdHandler : IRequestHandler<GetServicesByProviderIdQuery, Response<IEnumerable<ServiceDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetServicesByProviderIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<IEnumerable<ServiceDto>>> Handle(GetServicesByProviderIdQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<IEnumerable<ServiceDto>>();

        var provider = await _unitOfWork.Providers.GetByIdAsync(request.ProviderId, cancellationToken);
        if (provider == null)
        {
            response.IsSuccess = false;
            response.Message = "Provider not found.";
            return response;
        }

        var services = await _unitOfWork.Services.GetByProviderIdAsync(request.ProviderId, cancellationToken);
        
        var serviceDtos = _mapper.Map<IEnumerable<ServiceDto>>(services);

        response.IsSuccess = true;
        response.Message = $"Found {serviceDtos.Count()} services for provider '{provider.Name}'.";
        response.Data = serviceDtos;
        return response;
    }
}

