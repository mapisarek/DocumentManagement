using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.DAL;
using DocumentManagement.Models;
using DocumentManagement.Services;
using DocumentManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class DocumentController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        private readonly DocumentService _doc;
        private CategoryService _categories;
        private EmailClient _emailClient;
        private UserService _userService;
        private EmailService _emailService;

        public DocumentController(
            IHostingEnvironment appEnvironment,
            DMContext context)
        {
            _appEnvironment = appEnvironment;
            _doc = new DocumentService(context);
            _categories = new CategoryService(context);
            _userService = new UserService(context);
            _emailService = new EmailService(context, _appEnvironment.WebRootPath);
        }

        public async Task<IActionResult> Index(string str, int page, int pageSize, string cat)
        {

            if (page == 0 || page < 0)
                page = 1;
            if (pageSize == 0)
                pageSize = 5;
                var documentList = await _doc.GetDocuments(str, cat);

            return View(await PaginatedList<DocumentViewModel>.CreateAsync(documentList, page, pageSize));
        }

        public IActionResult Upload()
        { 
            ViewBag.categories = _categories.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, Document document, bool Notify)
        {
            _emailClient = new EmailClient();
            ViewBag.categories = _categories.GetAll();
            string extention = Path.GetExtension(file.FileName);
            if (extention == ".pdf" || extention == ".PDF" || extention == ".doc" || extention == ".DOC" || extention == ".docx" || extention == ".DOCX")
            {
                if (file == null || file.Length == 0 || file.Length > 4000000)
                {
                    ViewBag.error = "File either empty or max size exeeds.";
                    return View();
                }
                string path_Root = _appEnvironment.WebRootPath;
                if (_doc.DocumentUpload(file, path_Root, document))
                {
                    if (Notify)
                    {
                        _emailService.createNewEmail(document.DocumentTitle, document.DocumentDescription,Notify,null,null);
                    }

                    if(_emailClient.Counter > 0)
                    ViewBag.success = "Successfully document uploaded. Sent emails: " + _emailClient.Counter;
                }
            }
            else
            {
                ViewBag.error = ".pdf / .doc files are allowed only";
                return View();
            }
            return View();
        }


        public async Task<FileResult> Download(int id)
        {
            string filePath = _doc.getPath(id);
            string fileName = _doc.getName(id);
            var path = Path.Combine(
                                  Directory.GetCurrentDirectory(),
                                       "wwwroot\\Documents\\", fileName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        

        public ActionResult SendFile(int id)
        {
            try
            {
                ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
                _emailService.createNewEmail(_doc.getDocumentDetails(id).CategoryName, _doc.getDocumentDetails(id).DocumentDescription,
                    false, ViewBag.UserEmail, Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Documents\\", _doc.getName(id)));
                TempData["success"] = "Email was sent!";
            }
            catch
            {
                TempData["failed"] = "Something went wrong. Email not send";
            }
            return RedirectToAction("Details","Document",new { id = id });
        }

        public ActionResult Details(int id)
        {
            return View(_doc.getDocumentDetails(id));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

    }
}