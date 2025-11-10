using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;
using AutoMapper;

namespace TekusChallenge.Application.UseCases.Providers.Commands.UpdateProvider;

public sealed class UpdateProviderHandler : IRequestHandler<UpdateProviderCommand, Response<ProviderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProviderHandler(IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<ProviderDto>> Handle(UpdateProviderCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<ProviderDto>();
        
        var provider = await _unitOfWork.Providers.GetByIdAsync(request.Id, cancellationToken);
        if (provider == null)
        {
            response.IsSuccess = false;
            response.Message = "Provider not found.";
            return response;
        }

        var existingProviderByNit = await _unitOfWork.Providers.GetByNitAsync(request.Nit, cancellationToken);
        if (existingProviderByNit != null && existingProviderByNit.Id != request.Id)
        {
            response.IsSuccess = false;
            response.Message = "Another provider with the specified NIT already exists.";
            return response;
        }

        var existingProviderByEmail = await _unitOfWork.Providers.GetByEmailAsync(request.Email, cancellationToken);
        if (existingProviderByEmail != null && existingProviderByEmail.Id != request.Id)
        {
            response.IsSuccess = false;
            response.Message = "Another provider with the specified email already exists.";
            return response;
        }

        provider = _mapper.Map<Provider>(request);

        await _unitOfWork.Providers.UpdateAsync(provider!, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var providerDto = _mapper.Map<ProviderDto>(provider);

        response.IsSuccess = true;
        response.Message = "Provider updated successfully.";
        response.Data = providerDto;
        return response;
    }
}

