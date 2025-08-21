namespace TrackMyAssets_API.Domain.Exceptions;

public class UnitsMaxException : DomainException
{

    public UnitsMaxException(string message) :
        base(message)
    {

    }
}