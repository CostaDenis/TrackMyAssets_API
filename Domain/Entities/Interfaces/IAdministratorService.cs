using Blog.ViewModels;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities.DTOs;

namespace TrackMyAssets_API.Domain.Entities.Interfaces;

public interface IAdministratorService
{
    List<int> GetDataDashboard();
    List<User> GetAllUsers(int page = 0, int pageSize = 10);
    Administrator? GetAdministrator(Guid id);
    User? GetUserById(Guid id);
    Administrator? Login(LoginDTO loginDTO);
    void Update(Administrator administrator);
    void UpdatePassword(Administrator administrator, UpdatePasswordDTO updatePasswordDTO);
    void DeleteUser(User id);
    bool VerifyPassword(Administrator administrator, string hashedPassword, string providerPassword);

}
