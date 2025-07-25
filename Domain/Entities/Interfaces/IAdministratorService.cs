using TrackMyAssets_API.Domain.Entities.DTOs;

namespace TrackMyAssets_API.Domain.Entities.Interfaces;

public interface IAdministratorService
{
    Administrator? Login(LoginDTO loginDTO);
    List<User> GetAllUsers(int? page);
    User? GetUserById(Guid id);
    void DeleteUser(User id);

}
