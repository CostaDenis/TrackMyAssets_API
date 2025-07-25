using TrackMyAssets_API.Domain.Entities.DTOs;

namespace TrackMyAssets_API.Domain.Entities.Interfaces
{
    public interface IUserService
    {
        User? Login(LoginDTO loginDTO);
        bool VerifyPassword(User user, string hashedPassword, string providerPassword);
        User Create(string email, string password);
        User GetById(Guid id);
        void Update(User user);
        void DeleteOwnUser(User user);
        int CountUser();
    }
}