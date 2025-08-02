using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities.DTOs;

namespace TrackMyAssets_API.Domain.Entities.Interfaces;

public interface IUserService
{

    User? GetById(Guid id);
    User? Login(LoginDTO loginDTO);
    User Create(string email, string password);
    void Update(User user);
    ResultViewModel<string> UpdatePassword(User user, UpdatePasswordDTO updatePasswordDTO);
    ResultViewModel<string> UpdateEmail(User user, UpdateEmailDTO updateEmailDTO);
    void DeleteOwnUser(User user);
    bool VerifyPassword(User user, string hashedPassword, string providerPassword);
    int CountUser();
}
