using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities.DTOs;
using TrackMyAssets_API.Domain.Entities.Interfaces;

namespace TrackMyAssets_API.Domain.Entities.Services;

public class AdministratorService : IAdministratorService
{
    public AdministratorService(AppDbContext context)
    {
        _context = context;
    }

    private readonly AppDbContext _context;


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

    public List<User> GetAllUsers(int? page = 1)
    {
        var query = _context.Users.AsQueryable();
        int pageSize = 10;

        if (page != null)
            query = query.Skip(((int)page - 1) * pageSize).Take(pageSize);

        return query.ToList();
    }

    public User? GetUserById(Guid id)
    {
        var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();

        if (user == null)
            return null;

        return user;
    }

    public Administrator? GetAdministrator(Guid id)
    => _context.Administrators.Where(x => x.Id == id).FirstOrDefault();

    public void Update(Administrator administrator)
    {
        _context.Administrators.Update(administrator);
        _context.SaveChanges();
    }

    public ResultViewModel<string> UpdatePassword(Administrator administrator, UpdatePasswordDTO updatePasswordDTO)
    {
        if (!VerifyPassword(administrator, administrator.Password, updatePasswordDTO.CurrentPassword))
            return new ResultViewModel<string>("Verificação falha da senha atual!");

        if (updatePasswordDTO.NewPassword != updatePasswordDTO.NewPasswordConfirmation)
            return new ResultViewModel<string>("Confirmação falha da nova senha!");

        if (VerifyPassword(administrator, administrator.Password, updatePasswordDTO.NewPassword))
            return new ResultViewModel<string>("A nova senha não pode ser igual à anterior.");

        try
        {
            var hasher = new PasswordHasher<Administrator>();
            administrator.Password = hasher.HashPassword(administrator, updatePasswordDTO.NewPassword);
            Update(administrator);
            return new ResultViewModel<string>(data: "Senha atualizada com sucesso!");
        }
        catch
        {
            return new ResultViewModel<string>("Falha interna no servidor!");
        }
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
