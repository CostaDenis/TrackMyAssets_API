namespace TrackMyAssets_API.Domain.Exceptions;

public class InvalidPasswordException : DomainException
{

    public InvalidPasswordException(string message) :
        base(message)
    {

    }
}