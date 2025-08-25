namespace TrackMyAssets_API.Domain.Exceptions;

public class EmailConfirmationMismatchException : DomainException
{
    public EmailConfirmationMismatchException(string message) :
        base(message)
    {

    }
}