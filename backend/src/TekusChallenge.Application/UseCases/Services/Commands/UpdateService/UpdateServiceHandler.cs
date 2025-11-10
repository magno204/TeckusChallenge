using AutoMapper;
using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;
using TekusChallenge.Application.UseCases.Countries.Commands.SyncCountries;
using TekusChallenge.Application.UseCases.Countries.Queries.GetAllCountries;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Application.UseCases.Services.Commands.UpdateService
{
    public class UpdateServiceHandler : IRequestHandler<UpdateServiceCommand, Response<ServiceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateServiceHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Response<ServiceDto>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<ServiceDto>();

            var exists = await _unitOfWork.Services.AnyAsync(s => s.Id == request.Id, cancellationToken);
            if (!exists)
            {
                response.IsSuccess = false;
                response.Message = "Service not found.";
                return response;
            }

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
                    ServiceId = request.Id
                }).ToList();
            }

            await _unitOfWork.Services.UpdateAsync(service, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedService = await _unitOfWork.Services.GetByIdAsync(request.Id, cancellationToken);
            var serviceDto = _mapper.Map<ServiceDto>(updatedService);

            response.IsSuccess = true;
            response.Message = "Service updated successfully.";
            response.Data = serviceDto;
            return response;
        }
    }
}

