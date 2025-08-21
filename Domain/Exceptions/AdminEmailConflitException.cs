namespace TrackMyAssets_API.Domain.Exceptions;

public class AdminEmailConflitException : DomainException
{
    public AdminEmailConflitException(string message) :
        base(message)
    {

    }
}