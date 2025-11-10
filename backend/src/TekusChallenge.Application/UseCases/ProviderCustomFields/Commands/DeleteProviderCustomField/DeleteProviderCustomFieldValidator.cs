using FluentValidation;

namespace TekusChallenge.Application.UseCases.ProviderCustomFields.Commands.DeleteProviderCustomField;

public sealed class DeleteProviderCustomFieldValidator : AbstractValidator<DeleteProviderCustomFieldCommand>
{
    public DeleteProviderCustomFieldValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Custom field ID is required.");
    }
}

