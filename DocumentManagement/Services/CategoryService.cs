using DocumentManagement.DAL;
using DocumentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Services
{
    public class CategoryService
    {
        private readonly DMContext _context;
        public CategoryService(DMContext db)
        {
            _context = db;
        }
        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public bool CreateCategory(Category Cat)
        {
            bool status;
            Category item = new Category();
            item.CategoryName = Cat.CategoryName;
            try
            {
                _context.Categories.Add(item);

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

        public bool DeleteCategory(int id)
        {
            bool status;
            var item = _context.Categories.Find(id);

            try
            {
                _context.Categories.Remove(item);
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

        public int getCategoryAmount()
        {
            return _context.Categories.Count();
        }
    }
}
