namespace TrackMyAssets_API.Domain.Exceptions;

public class ZeroUnitsException : DomainException
{

    public ZeroUnitsException(string message) :
        base(message)
    {

    }
}