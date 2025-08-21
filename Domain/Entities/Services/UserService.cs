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

public class UserService : IUserService
{

    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public User? GetById(Guid id)
        => _context.Users.Where(x => x.Id == id).FirstOrDefault();

    public User? Login(LoginDTO loginDTO)
    {
        var user = _context.Users.FirstOrDefault(x => x.Email == loginDTO.Email);

        if (user == null)
            return null;

        if (VerifyPassword(user, user.Password, loginDTO.Password))
            return user;

        return null;
    }

    public User Create(string email, string password)
    {

        if (_context.Users.Any(x => x.Email == email))
            throw new EmailAlreadyExistsException(email);

        var emaildmin = _context.Administrators.Any(x => x.Email == email);

        if (emaildmin)
            throw new AdminEmailConflitException(email);

        var user = new User
        {
            Email = email,
            Password = string.Empty
        };

        var hasher = new PasswordHasher<User>();
        user.Password = hasher.HashPassword(user, password);

        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public ResultViewModel<string> UpdatePassword(User user, UpdatePasswordDTO updatePasswordDTO)
    {
        if (!VerifyPassword(user, user.Password, updatePasswordDTO.CurrentPassword))
            return new ResultViewModel<string>("Verificação falha da senha atual!");

        if (updatePasswordDTO.NewPassword != updatePasswordDTO.NewPasswordConfirmation)
            return new ResultViewModel<string>("Confirmação falha da nova senha!");

        if (VerifyPassword(user, user.Password, updatePasswordDTO.NewPassword))
            return new ResultViewModel<string>("A nova senha não pode ser igual à anterior.");

        try
        {
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, updatePasswordDTO.NewPassword);
            Update(user);
            return new ResultViewModel<string>(data: "Senha atualizada com sucesso!");
        }
        catch
        {
            return new ResultViewModel<string>("Falha interna no servidor!");
        }
    }

    public ResultViewModel<string> UpdateEmail(User user, UpdateEmailDTO updateEmailDTO)
    {
        if (user.Email == updateEmailDTO.NewEmail)
            return new ResultViewModel<string>("O novo email não pode ser igual à anterior.");

        var emaildmin = _context.Administrators.Any(x => x.Email == updateEmailDTO.NewEmail);

        if (emaildmin)
            return new ResultViewModel<string>("O novo email não pode ser igual ao email do Administrador!.");

        user.Email = updateEmailDTO.NewEmail;

        try
        {
            Update(user);
            return new ResultViewModel<string>(data: "Email atualizado com sucesso!");
        }
        catch (DbUpdateException)
        {
            return new ResultViewModel<string>("Email já em uso");
        }
        catch
        {
            return new ResultViewModel<string>("Falha interna no servidor!");
        }
    }

    public void DeleteOwnUser(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public bool VerifyPassword(User user, string hashedPassword, string providerPassword)
    {
        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, hashedPassword, providerPassword);

        if (result == PasswordVerificationResult.Failed)
            return false;

        return true;
    }

    public int CountUser()
        => _context.Users.Count();

}
