using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackMyAssets_API.Domain.Entities.DTOs;

namespace TrackMyAssets_API.Domain.Entities.Interfaces
{
    public interface IUserService
    {
        User? Login(LoginDTO loginDTO);
        void Create(User user);
        void Update(User user);
        void DeleteOwnUser(User user);
    }
}