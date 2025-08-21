namespace TrackMyAssets_API.Domain.Exceptions;

public class UnitsZeroException : DomainException
{

    public UnitsZeroException(string message) :
        base(message)
    {

    }
}