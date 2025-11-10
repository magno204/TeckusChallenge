using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TekusChallenge.Application.UseCases.Countries.Queries.GetAllCountries;
using TekusChallenge.Application.UseCases.Countries.Queries.GetCountryByCode;

namespace TekusChallenge.API.Controllers.v1;

/// <summary>
/// Controller for country management
/// Consumes data from the external REST Countries API (https://restcountries.com)
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor for the countries controller
    /// </summary>
    /// <param name="mediator">Mediator to send commands and queries</param>
    public CountriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets all countries from the external REST Countries API
    /// </summary>
    /// <returns>Complete list of countries with their ISO codes and flags</returns>
    /// <response code="200">Countries retrieved successfully</response>
    /// <response code="400">Error retrieving countries</response>
    /// <response code="503">External service unavailable</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetAllCountries()
    {
        var query = new GetAllCountriesQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            // If it's a connection error, return 503 Service Unavailable
            if (result.Message.Contains("conexión") || result.Message.Contains("API externa"))
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, result);
            }

            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets a specific country by its ISO Alpha-2 code from the external API
    /// </summary>
    /// <param name="code">ISO Alpha-2 country code (2 characters, e.g.: CO, MX, PE)</param>
    /// <returns>Information of the found country</returns>
    /// <response code="200">Country found successfully</response>
    /// <response code="400">Invalid or not found country code</response>
    /// <response code="503">External service unavailable</response>
    [HttpGet("{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetCountryByCode([FromRoute] string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return BadRequest("Country code is required");
        }

        var query = new GetCountryByCodeQuery { Code = code };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            // If it's a connection error, return 503 Service Unavailable
            if (result.Message.Contains("conexión") || result.Message.Contains("API externa"))
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, result);
            }

            return BadRequest(result);
        }

        return Ok(result);
    }
}
