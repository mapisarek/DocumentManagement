using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.DAL;
using DocumentManagement.Models;
using DocumentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Controllers
{
        public class CategoryController : Controller
        {
            private CategoryService _categories;

            public CategoryController(DMContext db)
            {
                _categories = new CategoryService(db);
            }

            [Authorize(Roles = "Admin")]
            public IActionResult Index()
            {
                var categories = _categories.GetAll();
                return View(categories);
            }

        [Authorize(Roles = "Admin")]
        [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public IActionResult Details()
        {
            var categories = _categories.GetAll();
            return View(categories);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
            public IActionResult Create(Category category)
            {
                var status = _categories.CreateCategory(category);
                if (status)
                {
                    ViewBag.success = "Created successfully";
                }
                else
                {
                    ViewBag.error = "Something was wrong.";
                }
                return View();
            }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
            {
                var status = _categories.DeleteCategory(id);
                if (status)
                {
                    TempData["success"] = "Deleted successfully";
                }
                else
                {
                    TempData["Error"] = "Error Occurred";
                }
                return RedirectToAction("Index");
            }
        }
}