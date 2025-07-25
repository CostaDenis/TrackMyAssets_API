using Blog.ViewModels;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities.DTOs;

namespace TrackMyAssets_API.Domain.Entities.Interfaces;

public interface IAdministratorService
{
    Administrator? Login(LoginDTO loginDTO);
    List<User> GetAllUsers(int? page);
    User? GetUserById(Guid id);
    Administrator? GetAdministrator(Guid id);
    void Update(Administrator administrator);
    ResultViewModel<string> UpdatePassword(Administrator administrator, UpdatePasswordDTO updatePasswordDTO);
    ResultViewModel<string> UpdateEmail(Administrator administrator, UpdateEmailDTO updateEmailDTO);
    void DeleteUser(User id);
    bool VerifyPassword(Administrator administrator, string hashedPassword, string providerPassword);

}
