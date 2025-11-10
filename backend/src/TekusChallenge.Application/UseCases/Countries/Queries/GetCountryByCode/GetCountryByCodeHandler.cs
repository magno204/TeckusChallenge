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

namespace TekusChallenge.Application.UseCases.Countries.Queries.GetCountryByCode;

public class GetCountryByCodeHandler : IRequestHandler<GetCountryByCodeQuery, Response<CountryDto>>
{
    private readonly IRestCountriesService _restCountriesService;
    private readonly IMapper _mapper;

    public GetCountryByCodeHandler(IRestCountriesService restCountriesService, IMapper mapper)
    {
        _restCountriesService = restCountriesService;
        _mapper = mapper;
    }

    public async Task<Response<CountryDto>> Handle(GetCountryByCodeQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<CountryDto>();

        try
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                response.IsSuccess = false;
                response.Message = "Country code is required.";
                return response;
            }

            var country = await _restCountriesService.FetchCountryByCodeAsync(request.Code, cancellationToken);
            
            if (country == null)
            {
                response.IsSuccess = false;
                response.Message = $"Country with code '{request.Code.ToUpper()}' not found in external API.";
                return response;
            }

            var countryDto = _mapper.Map<CountryDto>(country);

            response.IsSuccess = true;
            response.Message = $"Country '{country.Name}' successfully retrieved from external API.";
            response.Data = countryDto;
            return response;
        }
        catch (HttpRequestException)
        {
            response.IsSuccess = false;
            response.Message = "Connection error with external countries API. Please try again.";
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error retrieving country: {ex.Message}";
            return response;
        }
    }
}

