using Microsoft.AspNetCore.Identity;
using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.Entities.DTOs;
using TrackMyAssets_API.Domain.Entities.Interfaces;

namespace TrackMyAssets_API.Domain.Entities.Services
{
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

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}