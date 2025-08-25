using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities.DTOs;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Exceptions;

namespace TrackMyAssets_API.Domain.Entities.Services;

public class AdministratorService : IAdministratorService
{
    private readonly AppDbContext _context;
    private readonly IUserService _userService;
    private readonly IAssetService _assetService;
    private readonly IUserAssetService _userAssetService;

    public AdministratorService(
        AppDbContext context,
        IUserService userService,
        IAssetService assetService,
        IUserAssetService userAssetService
        )
    {
        _context = context;
        _userService = userService;
        _assetService = assetService;
        _userAssetService = userAssetService;
    }

    public List<int> GetDataDashboard()
    {
        List<int> data = [_userService.CountUser(), _assetService.CountAsset(), _userAssetService.CountUserAsset()];

        return data;
    }

    public List<User> GetAllUsers(int page = 1, int pageSize = 10)
    {
        var query = _context.Users.AsQueryable();
        query = query.Skip(((int)page - 1) * pageSize).Take(pageSize);

        return query.ToList();
    }

    public Administrator? GetAdministrator(Guid id)
    => _context.Administrators.Where(x => x.Id == id).FirstOrDefault();

    public User? GetUserById(Guid id)
    {
        var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();

        if (user == null)
            return null;

        return user;
    }

    public Administrator? Login(LoginDTO loginDTO)
    {
        var administrator = _context.Administrators.FirstOrDefault(x => x.Email == loginDTO.Email);

        if (administrator == null)
            return null;

        var hasher = new PasswordHasher<Administrator>();
        var result = hasher.VerifyHashedPassword(administrator, administrator.Password, loginDTO.Password);

        if (result == PasswordVerificationResult.Success)
            return administrator;

        return null;
    }

    public void Update(Administrator administrator)
    {
        _context.Administrators.Update(administrator);
        _context.SaveChanges();
    }

    public void UpdatePassword(Administrator administrator, UpdatePasswordDTO updatePasswordDTO)
    {
        if (!VerifyPassword(administrator, administrator.Password, updatePasswordDTO.CurrentPassword))
            throw new InvalidPasswordException("Erro ao conferir a atual senha");

        if (updatePasswordDTO.NewPassword != updatePasswordDTO.NewPasswordConfirmation)
            throw new PasswordConfirmationMismatchException("Erro ao confirmar a nova senha");

        if (VerifyPassword(administrator, administrator.Password, updatePasswordDTO.NewPassword))
            throw new PasswordReuseException("A nova senha não pode ser igual à anterior.");

        var hasher = new PasswordHasher<Administrator>();
        administrator.Password = hasher.HashPassword(administrator, updatePasswordDTO.NewPassword);
        Update(administrator);
    }

    public void DeleteUser(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public bool VerifyPassword(Administrator administrator, string hashedPassword, string providerPassword)
    {
        var hasher = new PasswordHasher<Administrator>();
        var result = hasher.VerifyHashedPassword(administrator, hashedPassword, providerPassword);

        if (result == PasswordVerificationResult.Failed)
            return false;

        return true;
    }

}
