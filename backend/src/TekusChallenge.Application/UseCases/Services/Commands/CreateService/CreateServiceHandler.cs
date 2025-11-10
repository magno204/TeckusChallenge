using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;
using TekusChallenge.Application.UseCases.Countries.Commands.SyncCountries;
using TekusChallenge.Application.UseCases.Countries.Queries.GetAllCountries;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Application.UseCases.Services.Commands.CreateService
{
    public class CreateServiceHandler : IRequestHandler<CreateServiceCommand, Response<ServiceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateServiceHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Response<ServiceDto>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<ServiceDto>();

            var provider = await _unitOfWork.Providers.GetByIdAsync(request.ProviderId, cancellationToken);
            if (provider == null)
            {
                response.IsSuccess = false;
                response.Message = "Provider not found.";
                return response;
            }

            if (request.CountryCodes != null && request.CountryCodes.Any())
            {
                var countriesResponse = await _mediator.Send(new GetAllCountriesQuery(), cancellationToken);
                if (!countriesResponse.IsSuccess)
                {
                    response.IsSuccess = false;
                    response.Message = "Error retrieving countries.";
                    return response;
                }
                var countries = countriesResponse.Data.ToDictionary(c => c.Code);
                foreach (var countryCode in request.CountryCodes)
                {
                    if (!countries.ContainsKey(countryCode))
                    {
                        response.IsSuccess = false;
                        response.Message = $"Country code '{countryCode}' is not valid.";
                        return response;
                    }
                }

                var syncCommand = new SyncCountriesCommand
                {
                    CountryCodes = request.CountryCodes,
                    CountriesFromApi = countries
                };

                var syncResult = await _mediator.Send(syncCommand, cancellationToken);
                if (!syncResult.IsSuccess)
                {
                    response.IsSuccess = false;
                    response.Message = $"Error synchronizing countries: {syncResult.Message}";
                    return response;
                }
            }

            var service = _mapper.Map<Service>(request);

            if (request.CountryCodes != null && request.CountryCodes.Any())
            {
                service.ServiceCountries = request.CountryCodes.Select(code => new ServiceCountry
                {
                    CountryCode = code,
                    ServiceId = service.Id
                }).ToList();
            }

            await _unitOfWork.Services.AddAsync(service, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var createdService = await _unitOfWork.Services.GetByIdAsync(service.Id, cancellationToken);
            var serviceDto = _mapper.Map<ServiceDto>(createdService);
            
            response.IsSuccess = true;
            response.Message = "Service created successfully.";
            response.Data = serviceDto;
            return response;
        }
    }
}

