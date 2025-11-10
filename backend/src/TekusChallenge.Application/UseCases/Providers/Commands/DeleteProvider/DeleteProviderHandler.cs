using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekus.Transversal;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Application.UseCases.Providers.Commands.DeleteProvider
{
    public class DeleteProviderHandler : IRequestHandler<DeleteProviderCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProviderHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(DeleteProviderCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();

            var provider = await _unitOfWork.Providers.GetByIdAsync(request.Id, cancellationToken);
            if (provider == null)
            {
                response.IsSuccess = false;
                response.Message = "Provider not found.";
                response.Data = false;
                return response;
            }

            await _unitOfWork.Providers.RemoveAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            response.IsSuccess = true;
            response.Message = "Provider deleted successfully.";
            response.Data = true;
            return response;
        }
    }
}

