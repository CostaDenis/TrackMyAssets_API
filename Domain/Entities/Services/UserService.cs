using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.Entities.DTOs;
using TrackMyAssets_API.Domain.Entities.Interfaces;

namespace TrackMyAssets_API.Domain.Entities.Services
{
    public class UserService : IUserService
    {

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        private readonly AppDbContext _context;

        public User? Login(LoginDTO loginDTO)
        {
            var user = _context.Users.Where(x => x.Email == loginDTO.Email
                && x.Password == loginDTO.Password).FirstOrDefault();

            return user;
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        public void Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteOwnUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

    }
}