using TrackMyAssets_API.Domain.Entities.DTOs;

namespace TrackMyAssets_API.Domain.Entities.Interfaces
{
    public interface IUserService
    {
        User? Login(LoginDTO loginDTO);
        User Create(string email, string password);
        User GetById(Guid id);
        void Update(User user);
        void DeleteOwnUser(User user);
    }
}