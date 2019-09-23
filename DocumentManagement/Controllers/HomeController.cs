using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DocumentManagement.Models;
using DocumentManagement.Services;
using DocumentManagement.DAL;

namespace DocumentManagement.Controllers
{
    public class HomeController : Controller
    {
        private Statistics statistics;
        private readonly DocumentService documentService;
        private readonly CategoryService categoryService;

        public HomeController(DMContext context)
        {
            documentService = new DocumentService(context);
            categoryService = new CategoryService(context);
        }


        public IActionResult Index()
        {
            statistics = new Statistics();
            statistics.CategoriesAmount = categoryService.getCategoryAmount();
            statistics.DocumentsAmount = documentService.getDocumentAmount();
            return View(statistics);
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
