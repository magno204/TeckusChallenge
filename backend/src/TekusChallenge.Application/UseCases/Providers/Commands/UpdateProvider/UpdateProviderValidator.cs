using FluentValidation;

namespace TekusChallenge.Application.UseCases.Providers.Commands.UpdateProvider;

public sealed class UpdateProviderValidator : AbstractValidator<UpdateProviderCommand>
{
    public UpdateProviderValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Provider ID is required.");

        RuleFor(x => x.Nit)
            .NotEmpty().WithMessage("NIT is required.")
            .MaximumLength(20).WithMessage("NIT cannot exceed 20 characters.")
            .Matches(@"^\d+$").WithMessage("NIT can only contain numbers.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.")
            .MinimumLength(2).WithMessage("Name must have at least 2 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email does not have a valid format.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

        RuleForEach(x => x.CustomFields)
            .ChildRules(customField =>
            {
                customField.RuleFor(x => x.FieldName)
                    .NotEmpty().WithMessage("Custom field name is required.")
                    .MaximumLength(100).WithMessage("Field name cannot exceed 100 characters.");

                customField.RuleFor(x => x.FieldValue)
                    .NotEmpty().WithMessage("Custom field value is required.")
                    .MaximumLength(500).WithMessage("Field value cannot exceed 500 characters.");

                customField.RuleFor(x => x.FieldType)
                    .NotEmpty().WithMessage("Field type is required.")
                    .Must(BeAValidFieldType).WithMessage("Field type must be one of: text, number, date, boolean, email, url.");

                customField.RuleFor(x => x.Description)
                    .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
                    .When(x => !string.IsNullOrWhiteSpace(x.Description));

                customField.RuleFor(x => x.DisplayOrder)
                    .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
            })
            .When(x => x.CustomFields != null && x.CustomFields.Any());
    }

    private bool BeAValidFieldType(string fieldType)
    {
        var validTypes = new[] { "text", "number", "date", "boolean", "email", "url" };
        return validTypes.Contains(fieldType.ToLowerInvariant());
    }
}

