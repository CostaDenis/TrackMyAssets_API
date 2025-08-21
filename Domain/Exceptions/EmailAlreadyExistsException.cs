namespace TrackMyAssets_API.Domain.Exceptions;

public class EmailAlreadyExistsException : DomainException
{

    public EmailAlreadyExistsException(string email) :
        base($"O e-mail '{email}' já está em uso.")
    {

    }
}