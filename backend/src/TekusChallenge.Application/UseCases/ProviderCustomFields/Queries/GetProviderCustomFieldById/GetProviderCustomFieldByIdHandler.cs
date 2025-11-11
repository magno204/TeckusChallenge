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

namespace TekusChallenge.Application.UseCases.ProviderCustomFields.Queries.GetProviderCustomFieldById;

/// <summary>
/// Handler to get a specific custom field by its ID
/// </summary>
public class GetProviderCustomFieldByIdHandler : IRequestHandler<GetProviderCustomFieldByIdQuery, Response<ProviderCustomFieldDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProviderCustomFieldByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<ProviderCustomFieldDto>> Handle(GetProviderCustomFieldByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<ProviderCustomFieldDto>();

        var customField = await _unitOfWork.ProviderCustomFields.GetByIdAsync(request.Id, cancellationToken);
        if (customField == null)
        {
            response.IsSuccess = false;
            response.Message = $"Custom field with ID '{request.Id}' not found.";
            return response;
        }

        var customFieldDto = _mapper.Map<ProviderCustomFieldDto>(customField);

        response.IsSuccess = true;
        response.Message = $"Custom field '{customField.FieldName}' found successfully.";
        response.Data = customFieldDto;
        
        return response;
    }
}

