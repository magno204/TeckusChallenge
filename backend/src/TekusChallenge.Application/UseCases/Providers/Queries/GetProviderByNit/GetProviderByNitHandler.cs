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

namespace TekusChallenge.Application.UseCases.Providers.Queries.GetProviderByNit;

public class GetProviderByNitHandler : IRequestHandler<GetProviderByNitQuery, Response<ProviderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProviderByNitHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<ProviderDto>> Handle(GetProviderByNitQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<ProviderDto>();

        if (string.IsNullOrWhiteSpace(request.Nit))
        {
            response.IsSuccess = false;
            response.Message = "NIT is required.";
            return response;
        }

        var provider = await _unitOfWork.Providers.GetByNitAsync(request.Nit, cancellationToken);
        if (provider == null)
        {
            response.IsSuccess = false;
            response.Message = "Provider not found.";
            return response;
        }

        var providerDto = _mapper.Map<ProviderDto>(provider);

        response.IsSuccess = true;
        response.Message = "Provider found successfully.";
        response.Data = providerDto;
        return response;
    }
}

