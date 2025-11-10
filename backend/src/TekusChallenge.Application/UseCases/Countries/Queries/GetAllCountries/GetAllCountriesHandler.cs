using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Application.UseCases.Countries.Queries.GetAllCountries;

public class GetAllCountriesHandler : IRequestHandler<GetAllCountriesQuery, Response<IEnumerable<CountryDto>>>
{
    private readonly IRestCountriesService _restCountriesService;
    private readonly IMapper _mapper;

    public GetAllCountriesHandler(
        IRestCountriesService restCountriesService,
        IMapper mapper)
    {
        _restCountriesService = restCountriesService;
        _mapper = mapper;
    }

    public async Task<Response<IEnumerable<CountryDto>>> Handle(
        GetAllCountriesQuery request,
        CancellationToken cancellationToken)
    {
        var response = new Response<IEnumerable<CountryDto>>();

        try
        {
            var countries = await _restCountriesService.FetchAllCountriesAsync(cancellationToken);

            var countryDtos = _mapper.Map<IEnumerable<CountryDto>>(countries);
            var countriesList = countryDtos.OrderBy(c => c.Name).ToList();

            response.Data = countriesList;
            response.IsSuccess = true;
            response.Message = $"{countriesList.Count} countries successfully retrieved from external API.";

            return response;
        }
        catch (HttpRequestException ex)
        {

            response.IsSuccess = false;
            response.Message = "Connection error with external countries API. Please try again.";
            response.Data = Enumerable.Empty<CountryDto>();
            return response;
        }
        catch (Exception ex)
        {

            response.IsSuccess = false;
            response.Message = $"Error retrieving countries: {ex.Message}";
            response.Data = Enumerable.Empty<CountryDto>();
            return response;
        }
    }
}
