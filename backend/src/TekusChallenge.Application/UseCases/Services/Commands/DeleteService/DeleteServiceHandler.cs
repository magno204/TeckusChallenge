using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekus.Transversal;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Application.UseCases.Services.Commands.DeleteService
{
    public class DeleteServiceHandler : IRequestHandler<DeleteServiceCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteServiceHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();

            var service = await _unitOfWork.Services.GetByIdAsync(request.Id, cancellationToken);
            if (service == null)
            {
                response.IsSuccess = false;
                response.Message = "Service not found.";
                response.Data = false;
                return response;
            }

            await _unitOfWork.Services.RemoveAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            response.IsSuccess = true;
            response.Message = "Service deleted successfully.";
            response.Data = true;
            return response;
        }
    }
}

