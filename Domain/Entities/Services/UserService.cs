using Microsoft.AspNetCore.Identity;
using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities.DTOs;
using TrackMyAssets_API.Domain.Entities.Interfaces;

namespace TrackMyAssets_API.Domain.Entities.Services
{
    public class UserService : IUserService
    {

        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public User? Login(LoginDTO loginDTO)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == loginDTO.Email);

            if (user == null)
                return null;

            if (VerifyPassword(user, user.Password, loginDTO.Password))
                return user;

            return null;
        }

        public bool VerifyPassword(User user, string hashedPassword, string providerPassword)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, hashedPassword, providerPassword);

            if (result == PasswordVerificationResult.Failed)
                return false;

            return true;
        }

        public User Create(string email, string password)
        {

            if (_context.Users.Any(x => x.Email == email))
                throw new InvalidOperationException("E-mail já está em uso.");

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

        public User GetById(Guid id)
        {
            return _context.Users.Where(x => x.Id == id).FirstOrDefault()!;
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteOwnUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public int CountUser()
            => _context.Users.Count();

    }
}