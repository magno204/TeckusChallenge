using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.ProviderCustomFields.Commands.CreateProviderCustomField
{
    public sealed record CreateProviderCustomFieldCommand: IRequest<Response<ProviderCustomFieldDto>>
    {
        public Guid ProviderId { get; init; }
        public string FieldName { get; init; } = string.Empty;
        public string FieldValue { get; init; } = string.Empty;
        public string FieldType { get; init; } = "text";
        public string? Description { get; init; }
        public int DisplayOrder { get; init; } = 0;
    }
}
