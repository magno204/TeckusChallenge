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

namespace TekusChallenge.Application.UseCases.Services.Queries.GetServiceById;

public class GetServiceByIdHandler : IRequestHandler<GetServiceByIdQuery, Response<ServiceDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetServiceByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<ServiceDto>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<ServiceDto>();

        var service = await _unitOfWork.Services.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
        {
            response.IsSuccess = false;
            response.Message = "Service not found.";
            return response;
        }

        var serviceDto = _mapper.Map<ServiceDto>(service);

        response.IsSuccess = true;
        response.Message = "Service found successfully.";
        response.Data = serviceDto;
        return response;
    }
}

