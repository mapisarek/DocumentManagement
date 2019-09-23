using DocumentManagement.DAL;
using DocumentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Services
{
    public class LoginService
    {
        private readonly DMContext _context;

        public LoginService(DMContext dataBase)
        {
            _context = dataBase;
        }

        public List<User> CheckCredential(UserLogin user)
        {
            var _user = _context.Users.FirstOrDefault(x => x.UserEmail == user.UserEmail
             && x.Password == user.Password);

            if (_user == null)
            {
                return null;
            }

            return _context.Users.Where(x => x.UserEmail == _user.UserEmail).Select(x => new User
            {
                UserId = x.UserId,
                UserName = x.UserName,
                UserEmail = x.UserEmail,
                UserRole = x.UserRole,
            }).ToList();
        }
    }
}

