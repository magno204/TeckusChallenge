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

namespace TekusChallenge.Application.UseCases.Providers.Queries.GetProviderById;

public class GetProviderHandler : IRequestHandler<GetProviderQuery, Response<ProviderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProviderHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<ProviderDto>> Handle(GetProviderQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<ProviderDto>();

        var provider = await _unitOfWork.Providers.GetByIdAsync(request.Id, cancellationToken);
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
