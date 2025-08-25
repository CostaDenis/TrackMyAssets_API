namespace TrackMyAssets_API.Domain.Exceptions;

public class EmailReuseException : DomainException
{

    public EmailReuseException(string message) :
        base(message)
    {

    }
}