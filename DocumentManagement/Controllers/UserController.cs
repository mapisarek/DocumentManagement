using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.DAL;
using DocumentManagement.Models;
using DocumentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Controllers
{
    [Authorize(Policy = "Admin")]
    public class UserController : Controller
    {
        private readonly UserService _user;
        public UserController(DMContext _context)
        {
            _user = new UserService(_context);
        }
        public IActionResult Index()
        {
            var users = _user.GetAll();
            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(User user)
        {
            var status = _user.Create(user);
            if (status)
            {
                ViewBag.success = "Created successfully";
            }
            else
            {
                ViewBag.error = "Error Occurred";
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            var status = _user.Delete(id);
            if (status)
            {
                ViewBag.success = "Deleted successfully";
            }
            else
            {
                ViewBag.error = "Error Occurred";
            }
            return RedirectToAction("Index");
        }
    }
}