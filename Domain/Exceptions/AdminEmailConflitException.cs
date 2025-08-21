namespace TrackMyAssets_API.Domain.Exceptions;

public class AdminEmailConflitException : DomainException
{
    public AdminEmailConflitException(string email) :
        base($"O e-mail '{email}' não pode ser igual ao do administrador.")
    {

    }
}