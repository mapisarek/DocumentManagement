using DocumentManagement.DAL;
using DocumentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Services
{
    public class UserService
    {
        private readonly DMContext _context;
        public UserService(DMContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            var _users = _context.Users.ToList();
            return _users;
        }

        public bool Create(User user)
        {
            bool status;
            User item = new User();
            item.UserName = user.UserName;
            item.UserEmail = user.UserEmail;
            item.Password = user.Password;
            item.UserRole = user.UserRole;
            try
            {
                _context.Users.Add(item);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                var exp = ex;
                status = false;
            }
            return status;
        }

        public bool Delete(int id)
        {
            bool status;
            var item = _context.Users.Find(id);
            try
            {
                _context.Users.Remove(item);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                var exp = ex;
                status = false;
            }
            return status;
        }

        public List<string> GetAllEmails()
        {
            var allEmails = _context.Users.Select(x => x.UserEmail).ToList();
            return allEmails;
        }

    }
}
