using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;
using TekusChallenge.Domain.Interfaces;
using AutoMapper;
using TekusChallenge.Domain.Entities;

namespace TekusChallenge.Application.UseCases.ProviderCustomFields.Commands.CreateProviderCustomField
{
    public class CreateProviderCustomFieldHandler : IRequestHandler<CreateProviderCustomFieldCommand, Response<ProviderCustomFieldDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateProviderCustomFieldHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<ProviderCustomFieldDto>> Handle(CreateProviderCustomFieldCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<ProviderCustomFieldDto>();

            var provider = await _unitOfWork.ProviderCustomFields.GetByIdAsync(request.ProviderId, cancellationToken);
            if (provider == null)
            {
                response.IsSuccess = false;
                response.Message = "Provider not found.";
                return response;
            }

            var providerCustomField = _mapper.Map<ProviderCustomField>(request);

            await _unitOfWork.ProviderCustomFields.AddAsync(providerCustomField, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var providerCustomFieldDto = _mapper.Map<ProviderCustomFieldDto>(providerCustomField);
            
            response.IsSuccess = true;
            response.Message = "Custom field created successfully.";
            response.Data = providerCustomFieldDto;
            return response;
        }
    }
}
