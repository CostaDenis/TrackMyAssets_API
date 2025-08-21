namespace TrackMyAssets_API.Domain.Exceptions;

public class InsufficientBalance : DomainException
{

    public InsufficientBalance(string message) :
        base(message)
    {

    }
}