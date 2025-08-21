namespace TrackMyAssets_API.Domain.Exceptions;

public class InvalidIdException : DomainException
{

    public InvalidIdException(string message) :
        base(message)
    {

    }
}