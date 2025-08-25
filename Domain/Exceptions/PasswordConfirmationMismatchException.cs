namespace TrackMyAssets_API.Domain.Exceptions;

public class PasswordConfirmationMismatchException : DomainException
{

    public PasswordConfirmationMismatchException(string message) :
        base(message)
    {

    }
}