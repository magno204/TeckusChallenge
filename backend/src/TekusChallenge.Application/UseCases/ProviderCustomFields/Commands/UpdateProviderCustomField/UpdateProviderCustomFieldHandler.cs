using AutoMapper;
using MediatR;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.ProviderCustomFields.Commands.UpdateProviderCustomField
{
    public class UpdateProviderCustomFieldHandler : IRequestHandler<UpdateProviderCustomFieldCommand, Response<ProviderCustomFieldDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProviderCustomFieldHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<ProviderCustomFieldDto>> Handle(UpdateProviderCustomFieldCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<ProviderCustomFieldDto>();

            var existingField = await _unitOfWork.ProviderCustomFields.GetByIdAsync(request.Id, cancellationToken);
            if (existingField == null)
            {
                response.IsSuccess = false;
                response.Message = "Custom field not found.";
                return response;
            }

            var provider = await _unitOfWork.Providers.GetByIdAsync(request.ProviderId, cancellationToken);
            if (provider == null)
            {
                response.IsSuccess = false;
                response.Message = "Provider not found.";
                return response;
            }

            var providerCustomField = _mapper.Map<ProviderCustomField>(request);

            await _unitOfWork.ProviderCustomFields.UpdateAsync(providerCustomField, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var providerCustomFieldDto = _mapper.Map<ProviderCustomFieldDto>(providerCustomField);

            response.IsSuccess = true;
            response.Message = "Custom field updated successfully.";
            response.Data = providerCustomFieldDto;
            return response;
        }
    }
}

