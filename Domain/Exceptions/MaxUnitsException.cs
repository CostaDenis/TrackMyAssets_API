namespace TrackMyAssets_API.Domain.Exceptions;

public class MaxUnitsException : DomainException
{

    public MaxUnitsException(string message) :
        base(message)
    {

    }
}