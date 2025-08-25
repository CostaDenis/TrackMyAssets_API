using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
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
            throw new AdminEmailConflitException("O email não pode ser igual ao do administrador.");

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

    public void UpdatePassword(User user, UpdatePasswordDTO updatePasswordDTO)
    {
        if (!VerifyPassword(user, user.Password, updatePasswordDTO.CurrentPassword))
            throw new InvalidPasswordException("Erro ao conferir a atual senha");

        if (updatePasswordDTO.NewPassword != updatePasswordDTO.NewPasswordConfirmation)
            throw new PasswordConfirmationMismatchException("Erro ao confirmar a nova senha");

        if (VerifyPassword(user, user.Password, updatePasswordDTO.NewPassword))
            throw new PasswordReuseException("A nova senha não pode ser igual à anterior.");

        var hasher = new PasswordHasher<User>();
        user.Password = hasher.HashPassword(user, updatePasswordDTO.NewPassword);
        Update(user);
    }

    public void UpdateEmail(User user, UpdateEmailDTO updateEmailDTO)
    {
        if (user.Email == updateEmailDTO.NewEmail)
            throw new EmailReuseException("O novo email não pode ser igual à anterior.");

        if (updateEmailDTO.NewEmail != updateEmailDTO.NewEmailConfirmation)
            throw new EmailConfirmationMismatchException("Erro ao confirmar o email");

        var emaildmin = _context.Administrators.Any(x => x.Email == updateEmailDTO.NewEmail);

        if (emaildmin)
            throw new AdminEmailConflitException("O email não pode ser igual ao do administrador.");

        user.Email = updateEmailDTO.NewEmail;
        Update(user);
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
