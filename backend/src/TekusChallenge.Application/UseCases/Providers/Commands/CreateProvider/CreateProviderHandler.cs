using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;
using AutoMapper;

namespace TekusChallenge.Application.UseCases.Providers.Commands.CreateProvider;

public sealed class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand, Response<ProviderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public CreateProviderCommandHandler(IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<ProviderDto>> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<ProviderDto>();

        var existingProviderByNit = await _unitOfWork.Providers.GetByNitAsync(request.Nit, cancellationToken);
        if (existingProviderByNit != null)
        {
            response.IsSuccess = false;
            response.Message = "A provider with the specified NIT already exists.";
            return response;
        }

        var existingProviderByEmail = await _unitOfWork.Providers.GetByEmailAsync(request.Email, cancellationToken);
        if (existingProviderByEmail != null)
        {
            response.IsSuccess = false;
            response.Message = "A provider with the specified email already exists.";
            return response;
        }

        var provider = _mapper.Map<Provider>(request);

        await _unitOfWork.Providers.AddAsync(provider, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if(request.CustomFields != null && request.CustomFields.Any())
        {
            foreach (var customFieldDto in request.CustomFields)
            {
                var customField = new ProviderCustomField
                {
                    Id = Guid.NewGuid(),
                    ProviderId = provider.Id,
                    FieldName = customFieldDto.FieldName,
                    FieldValue = customFieldDto.FieldValue,
                    FieldType = customFieldDto.FieldType,
                    Description = customFieldDto.Description,
                    DisplayOrder = customFieldDto.DisplayOrder
                };
                await _unitOfWork.ProviderCustomFields.AddAsync(customField, cancellationToken);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var providerDto = _mapper.Map<ProviderDto>(provider);

        response.IsSuccess = true;
        response.Message = "Provider created successfully.";
        response.Data = providerDto;
        return response;
    }
}

