using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TekusChallenge.Application.UseCases.Services.Commands.CreateService;
using TekusChallenge.Application.UseCases.Services.Commands.UpdateService;
using TekusChallenge.Application.UseCases.Services.Commands.DeleteService;
using TekusChallenge.Application.UseCases.Services.Queries.GetAllServices;
using TekusChallenge.Application.UseCases.Services.Queries.GetServiceById;
using TekusChallenge.Application.UseCases.Services.Queries.GetServicesByProviderId;

namespace TekusChallenge.API.Controllers.v1;

/// <summary>
/// Controller for service management
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor for the services controller
    /// </summary>
    /// <param name="mediator">Mediator to send commands and queries</param>
    public ServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new service
    /// </summary>
    /// <param name="command">Command with service data</param>
    /// <returns>Created service</returns>
    [HttpPost]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceCommand command)
    {
        if (command == null)
        {
            return BadRequest("Command cannot be null");
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    /// <summary>
    /// Updates an existing service
    /// </summary>
    /// <param name="id">Service ID</param>
    /// <param name="command">Command with updated data</param>
    /// <returns>Updated service</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService(string id, [FromBody] UpdateServiceCommand command)
    {
        var serviceId = Guid.Parse(id);
        if (serviceId == Guid.Empty)
        {
            return BadRequest("Service ID is required");
        }

        if (command == null)
        {
            return BadRequest("Command is required");
        }

        if (serviceId != command.Id)
        {
            return BadRequest("Service ID does not match command ID");
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    /// <summary>
    /// Deletes a service
    /// </summary>
    /// <param name="id">ID of the service to delete</param>
    /// <returns>Operation result</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Service ID is required");
        }

        var serviceId = Guid.Parse(id);

        var result = await _mediator.Send(new DeleteServiceCommand { Id = serviceId });
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets all services with pagination and filters
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="searchTerm">Search term</param>
    /// <param name="providerId">Provider ID</param>
    /// <param name="orderBy">Sort field</param>
    /// <param name="orderDescending">Descending order</param>
    /// <returns>Paginated list of services</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllServices(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] Guid? providerId = null,
        [FromQuery] string? orderBy = null,
        [FromQuery] bool orderDescending = false)
    {
        var query = new GetAllServicesQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            ProviderId = providerId,
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

    /// <summary>
    /// Gets a service by its ID
    /// </summary>
    /// <param name="id">Service ID</param>
    /// <returns>Found service</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetServiceById([FromRoute] string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Service ID is required");
        }

        var serviceId = Guid.Parse(id);

        var result = await _mediator.Send(new GetServiceByIdQuery { Id = serviceId });
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets all services from a specific provider
    /// </summary>
    /// <param name="providerId">Provider ID</param>
    /// <returns>List of provider's services</returns>
    [HttpGet("provider/{providerId}")]
    public async Task<IActionResult> GetServicesByProviderId([FromRoute] string providerId)
    {
        if (string.IsNullOrEmpty(providerId))
        {
            return BadRequest("Provider ID is required");
        }

        var providerGuid = Guid.Parse(providerId);

        var result = await _mediator.Send(new GetServicesByProviderIdQuery { ProviderId = providerGuid });
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
}
