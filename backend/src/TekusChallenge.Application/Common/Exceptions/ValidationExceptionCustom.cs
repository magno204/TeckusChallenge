using Tekus.Transversal;

namespace TekusChallenge.Application.Common.Exceptions;

public class ValidationExceptionCustom : Exception
{
    public IEnumerable<BaseError>? Errors { get; set; }

    public ValidationExceptionCustom() : base("One or more validation errors have occurred.")
    {
        Errors = new List<BaseError>();
    }

    public ValidationExceptionCustom(string message) : base(message)
    {
    }

    public ValidationExceptionCustom(IEnumerable<BaseError> errors) : this()
    {
        Errors = errors;
    }

}
