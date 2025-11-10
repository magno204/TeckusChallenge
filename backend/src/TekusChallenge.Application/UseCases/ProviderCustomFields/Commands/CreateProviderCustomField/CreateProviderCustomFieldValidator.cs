using FluentValidation;

namespace TekusChallenge.Application.UseCases.ProviderCustomFields.Commands.CreateProviderCustomField;

public sealed class CreateProviderCustomFieldValidator : AbstractValidator<CreateProviderCustomFieldCommand>
{
    public CreateProviderCustomFieldValidator()
    {
        RuleFor(x => x.ProviderId)
            .NotEmpty().WithMessage("Provider ID is required.");

        RuleFor(x => x.FieldName)
            .NotEmpty().WithMessage("Custom field name is required.")
            .MaximumLength(100).WithMessage("Field name cannot exceed 100 characters.");

        RuleFor(x => x.FieldValue)
            .NotEmpty().WithMessage("Custom field value is required.")
            .MaximumLength(500).WithMessage("Field value cannot exceed 500 characters.");

        RuleFor(x => x.FieldType)
            .NotEmpty().WithMessage("Field type is required.")
            .Must(BeAValidFieldType).WithMessage("Field type must be one of: text, number, date, boolean, email, url.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
    }

    private bool BeAValidFieldType(string fieldType)
    {
        var validTypes = new[] { "text", "number", "date", "boolean", "email", "url" };
        return validTypes.Contains(fieldType.ToLowerInvariant());
    }
}
