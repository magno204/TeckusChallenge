namespace TekusChallenge.Domain.Interfaces;

public interface IAuthenticationSettings
{
    string GetValidUsername();
    string GetValidPassword();
}