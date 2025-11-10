using FluentValidation;

namespace TekusChallenge.Application.UseCases.Providers.Commands.DeleteProvider;

public sealed class DeleteProviderValidator : AbstractValidator<DeleteProviderCommand>
{
    public DeleteProviderValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Provider ID is required.");
    }
}

