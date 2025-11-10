using FluentValidation;

namespace TekusChallenge.Application.UseCases.Services.Commands.UpdateService;

public sealed class UpdateServiceValidator : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Service ID is required.");

        RuleFor(x => x.ProviderId)
            .NotEmpty().WithMessage("Provider ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Service name is required.")
            .MaximumLength(200).WithMessage("Service name cannot exceed 200 characters.");

        RuleFor(x => x.HourlyRate)
            .NotEmpty().WithMessage("Hourly rate is required.")
            .GreaterThan(0).WithMessage("Hourly rate must be greater than 0.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

