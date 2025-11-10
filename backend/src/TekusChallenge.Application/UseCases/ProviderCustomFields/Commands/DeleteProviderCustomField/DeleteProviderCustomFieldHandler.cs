using MediatR;
using TekusChallenge.Domain.Interfaces;
using Tekus.Transversal;

namespace TekusChallenge.Application.UseCases.ProviderCustomFields.Commands.DeleteProviderCustomField;

public class DeleteProviderCustomFieldHandler : IRequestHandler<DeleteProviderCustomFieldCommand, Response<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProviderCustomFieldHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<bool>> Handle(DeleteProviderCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<bool>();

        var providerCustomField = await _unitOfWork.ProviderCustomFields.GetByIdAsync(request.Id, cancellationToken);
        if (providerCustomField == null)
        {
            response.IsSuccess = false;
            response.Message = "Custom field not found.";
            response.Data = false;
            return response;
        }

        await _unitOfWork.ProviderCustomFields.RemoveAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        response.IsSuccess = true;
        response.Message = "Custom field deleted successfully.";
        response.Data = true;
        return response;
    }
}

