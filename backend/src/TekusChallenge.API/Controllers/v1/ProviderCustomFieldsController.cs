using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TekusChallenge.Application.UseCases.ProviderCustomFields.Commands.CreateProviderCustomField;
using TekusChallenge.Application.UseCases.ProviderCustomFields.Commands.UpdateProviderCustomField;
using TekusChallenge.Application.UseCases.ProviderCustomFields.Commands.DeleteProviderCustomField;
using TekusChallenge.Application.UseCases.ProviderCustomFields.Queries.GetProviderCustomFieldsByProviderId;
using TekusChallenge.Application.UseCases.ProviderCustomFields.Queries.GetProviderCustomFieldById;

namespace TekusChallenge.API.Controllers.v1;

/// <summary>
/// Controller for managing custom fields of providers
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class ProviderCustomFieldsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the ProviderCustomFieldsController
    /// </summary>
    /// <param name="mediator">Mediator for CQRS pattern</param>
    public ProviderCustomFieldsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new custom field for a provider
    /// </summary>
    /// <param name="command">Command to create a custom field</param>
    /// <returns>Operation result with the created custom field</returns>
    /// <response code="200">Custom field created successfully</response>
    /// <response code="400">Invalid request or validation error</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProviderCustomField([FromBody] CreateProviderCustomFieldCommand command)
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
    /// Updates an existing custom field
    /// </summary>
    /// <param name="id">ID of the custom field to update</param>
    /// <param name="command">Command to update the custom field</param>
    /// <returns>Operation result with the updated custom field</returns>
    /// <response code="200">Custom field updated successfully</response>
    /// <response code="400">Invalid request or validation error</response>
    /// <response code="404">Custom field not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProviderCustomField(string id, [FromBody] UpdateProviderCustomFieldCommand command)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("ID is required");
        }

        if (!Guid.TryParse(id, out var customFieldId))
        {
            return BadRequest("Invalid ID format");
        }

        if (command == null)
        {
            return BadRequest("Command is required");
        }

        // Asegurar que el ID del comando coincida con el ID de la ruta
        if (command.Id != customFieldId)
        {
            return BadRequest("ID in route does not match ID in command");
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    /// <summary>
    /// Deletes an existing custom field
    /// </summary>
    /// <param name="id">ID of the custom field to delete</param>
    /// <returns>Operation result</returns>
    /// <response code="200">Custom field deleted successfully</response>
    /// <response code="400">Invalid request</response>
    /// <response code="404">Custom field not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProviderCustomField(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("ID is required");
        }

        if (!Guid.TryParse(id, out var customFieldId))
        {
            return BadRequest("Invalid ID format");
        }

        var result = await _mediator.Send(new DeleteProviderCustomFieldCommand { Id = customFieldId });

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets all custom fields for a specific provider
    /// </summary>
    /// <param name="providerId">ID of the provider</param>
    /// <returns>Operation result with the list of custom fields</returns>
    /// <response code="200">Custom fields retrieved successfully</response>
    /// <response code="400">Invalid request</response>
    /// <response code="404">Provider not found</response>
    [HttpGet("provider/{providerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProviderCustomFieldsByProviderId(string providerId)
    {
        if (string.IsNullOrEmpty(providerId))
        {
            return BadRequest("Provider ID is required");
        }

        if (!Guid.TryParse(providerId, out var providerGuid))
        {
            return BadRequest("Invalid Provider ID format");
        }

        var result = await _mediator.Send(new GetProviderCustomFieldsByProviderIdQuery { ProviderId = providerGuid });

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets a specific custom field by its ID
    /// </summary>
    /// <param name="id">ID of the custom field to retrieve</param>
    /// <returns>Operation result with the custom field</returns>
    /// <response code="200">Custom field retrieved successfully</response>
    /// <response code="400">Invalid request</response>
    /// <response code="404">Custom field not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProviderCustomFieldById(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("ID is required");
        }

        if (!Guid.TryParse(id, out var customFieldId))
        {
            return BadRequest("Invalid ID format");
        }

        var result = await _mediator.Send(new GetProviderCustomFieldByIdQuery { Id = customFieldId });

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
}

