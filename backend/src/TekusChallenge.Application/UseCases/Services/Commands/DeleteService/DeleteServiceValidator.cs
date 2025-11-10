using FluentValidation;

namespace TekusChallenge.Application.UseCases.Services.Commands.DeleteService;

public sealed class DeleteServiceValidator : AbstractValidator<DeleteServiceCommand>
{
    public DeleteServiceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Service ID is required.");
    }
}

