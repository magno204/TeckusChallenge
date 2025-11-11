using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekusChallenge.Application.UseCases.Statistics.Queries.GetProvidersByCountry;
using TekusChallenge.Application.UseCases.Statistics.Queries.GetServicesByCountry;
using TekusChallenge.Application.UseCases.Statistics.Queries.GetSummaryReport;

namespace TekusChallenge.API.Controllers.v1;

/// <summary>
/// Controller for statistics and key indicators
/// Provides summary reports and analytics about providers, services, and countries
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
[Authorize]
public class StatisticsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor for the statistics controller
    /// </summary>
    /// <param name="mediator">Mediator to send commands and queries</param>
    public StatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a comprehensive summary report with all key indicators
    /// </summary>
    /// <returns>Summary report with statistics by country, totals, and rankings</returns>
    /// <response code="200">Summary report retrieved successfully</response>
    /// <response code="400">Error retrieving summary report</response>
    /// <remarks>
    /// This endpoint provides:
    /// - Statistics grouped by country (providers and services count per country)
    /// - Total providers and services in the system
    /// - Total countries covered
    /// - Average hourly rate
    /// - Most expensive and cheapest services
    /// </remarks>
    [HttpGet("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSummaryReport()
    {
        var query = new GetSummaryReportQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets the count of providers grouped by country
    /// </summary>
    /// <returns>List of countries with their provider counts</returns>
    /// <response code="200">Providers statistics retrieved successfully</response>
    /// <response code="400">Error retrieving providers statistics</response>
    /// <remarks>
    /// This endpoint shows how many providers offer services in each country.
    /// A provider is counted for a country if they offer at least one service in that country.
    /// </remarks>
    [HttpGet("providers-by-country")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProvidersByCountry()
    {
        var query = new GetProvidersByCountryQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets the count of services grouped by country
    /// </summary>
    /// <returns>List of countries with their service counts and average rates</returns>
    /// <response code="200">Services statistics retrieved successfully</response>
    /// <response code="400">Error retrieving services statistics</response>
    /// <remarks>
    /// This endpoint shows how many services are available in each country.
    /// A service can be counted in multiple countries if it's offered internationally.
    /// </remarks>
    [HttpGet("services-by-country")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetServicesByCountry()
    {
        var query = new GetServicesByCountryQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}

