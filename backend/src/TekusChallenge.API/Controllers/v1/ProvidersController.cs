using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TekusChallenge.Application.UseCases.Providers.Commands.CreateProvider;
using TekusChallenge.Application.UseCases.Providers.Commands.UpdateProvider;
using TekusChallenge.Application.UseCases.Providers.Commands.DeleteProvider;
using TekusChallenge.Application.UseCases.Providers.Queries.GetProviderById;
using TekusChallenge.Application.UseCases.Providers.Queries.GetProviderByNit;
using TekusChallenge.Application.UseCases.Providers.Queries.GetAllProviders;

namespace TekusChallenge.API.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class ProvidersController: ControllerBase
{
    private readonly IMediator _mediator;

    public ProvidersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new provider
    /// </summary>
    /// <param name="command">Command to create a provider</param>
    /// <returns>Operation result</returns>
    [HttpPost]
    public async Task<IActionResult> CreateProvider([FromBody] CreateProviderCommand command)
    {
        if(command == null)
        {
            return BadRequest();
        }

        var result = await _mediator.Send(command);

        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    /// <summary>
    /// Updates an existing provider
    /// </summary>
    /// <param name="id">ID of the provider to update</param>
    /// <param name="command">Command to update a provider</param>
    /// <returns>Operation result</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProvider(string id, [FromBody] UpdateProviderCommand command)
    {
        var providerId = Guid.Parse(id);
        if(providerId == Guid.Empty)
        {
            return BadRequest("ID provider is required");
        }

        if(command == null)
        {
            return BadRequest("Command is required");
        }
        
        var result = await _mediator.Send(command);

        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Deletes an existing provider
    /// </summary>
    /// <param name="id">ID of the provider to delete</param>
    /// <returns>Operation result</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProvider(string id)
    {
        if(string.IsNullOrEmpty(id))
        {
            return BadRequest("ID provider is required");
        }

        var providerId = Guid.Parse(id);

        var result = await _mediator.Send(new DeleteProviderCommand { Id = providerId });
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Gets a provider by its ID
    /// </summary>
    /// <param name="id">ID of the provider to retrieve</param>
    /// <returns>Operation result</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProviderById([FromRoute] string id)
    {
        if(string.IsNullOrEmpty(id))
        {
            return BadRequest("ID provider is required");
        }

        var providerId = Guid.Parse(id);

        var result = await _mediator.Send(new GetProviderQuery { Id = providerId });
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Gets a provider by its NIT
    /// </summary>
    /// <param name="nit">NIT of the provider to retrieve</param>
    /// <returns>Operation result</returns>
    [HttpGet("by-nit/{nit}")]
    public async Task<IActionResult> GetProviderByNit([FromRoute] string nit)
    {
        if(string.IsNullOrEmpty(nit))
        {
            return BadRequest("NIT provider is required");
        }

        var result = await _mediator.Send(new GetProviderByNitQuery { Nit = nit });
        if(!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Gets all providers
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="searchTerm">Search term</param>
    /// <param name="nit">Provider's NIT</param>
    /// <param name="email">Provider's email</param>
    /// <param name="orderBy">Sort field</param>
    /// <param name="orderDescending">Descending order</param>
    /// <returns>Operation result</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllProviders(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? nit = null,
        [FromQuery] string? email = null,
        [FromQuery] string? orderBy = null,
        [FromQuery] bool orderDescending = false)
    {
        var query = new GetAllProvidersQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            Nit = nit,
            Email = email,
            OrderBy = orderBy,
            OrderDescending = orderDescending
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
}
