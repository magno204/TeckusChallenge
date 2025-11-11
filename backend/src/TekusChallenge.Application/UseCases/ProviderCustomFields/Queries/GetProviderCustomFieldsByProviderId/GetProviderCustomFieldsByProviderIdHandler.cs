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

namespace TekusChallenge.Application.UseCases.ProviderCustomFields.Queries.GetProviderCustomFieldsByProviderId;

/// <summary>
/// Handler to get all custom fields for a specific provider
/// </summary>
public class GetProviderCustomFieldsByProviderIdHandler : IRequestHandler<GetProviderCustomFieldsByProviderIdQuery, Response<IEnumerable<ProviderCustomFieldDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProviderCustomFieldsByProviderIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<IEnumerable<ProviderCustomFieldDto>>> Handle(GetProviderCustomFieldsByProviderIdQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<IEnumerable<ProviderCustomFieldDto>>();

        var provider = await _unitOfWork.Providers.GetByIdAsync(request.ProviderId, cancellationToken);
        if (provider == null)
        {
            response.IsSuccess = false;
            response.Message = $"Provider with ID '{request.ProviderId}' not found.";
            return response;
        }

        var customFields = await _unitOfWork.ProviderCustomFields.GetByProviderIdAsync(request.ProviderId, cancellationToken);
        
        var customFieldDtos = _mapper.Map<IEnumerable<ProviderCustomFieldDto>>(customFields)
                                     .OrderBy(cf => cf.DisplayOrder)
                                     .ToList();

        response.IsSuccess = true;
        response.Message = customFieldDtos.Count > 0 
            ? $"Found {customFieldDtos.Count} custom field(s) for provider '{provider.Name}'." 
            : $"No custom fields found for provider '{provider.Name}'.";
        response.Data = customFieldDtos;
        
        return response;
    }
}

