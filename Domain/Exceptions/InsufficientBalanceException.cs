namespace TrackMyAssets_API.Domain.Exceptions;

public class InsufficientBalanceException : DomainException
{

    public InsufficientBalanceException(string message) :
        base(message)
    {

    }
}