namespace TrackMyAssets_API.Domain.Exceptions;

public class PasswordReuseException : DomainException
{

    public PasswordReuseException(string message) :
        base(message)
    {

    }
}