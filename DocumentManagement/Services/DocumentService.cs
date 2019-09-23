using DocumentManagement.DAL;
using DocumentManagement.Models;
using DocumentManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Services
{
    public class DocumentService
    {
        private readonly DMContext _context;
        private readonly IHostingEnvironment _appEnvironment;
        private DocumentViewModel documentVM;

        public DocumentService(DMContext db)
        {
            _context = db;
        }

        public DocumentService(DMContext db, IHostingEnvironment appEnvironment)
        {
            _context = db;
            _appEnvironment = appEnvironment;
        }

        //get documents 
        public async Task<IQueryable<DocumentViewModel>> GetDocuments(string str, string cat)
        {
            var doc = from x in _context.Documents
                      select x;
            var items = from x in _context.Documents
                        select new DocumentViewModel
                        {
                            DocumentId = x.DocumentId,
                            DocumentTitle = x.DocumentTitle,
                            DocumentName = x.DocumentName,
                            CategoryId = x.CategoryId,
                            CategoryName = x.Category.CategoryName
                        };
            if (!string.IsNullOrEmpty(str) && string.IsNullOrEmpty(cat))
            {
                var searcheditems = from x in doc.Where(x => x.DocumentTitle.Contains(str) || x.DocumentName.Contains(str) || x.Category.CategoryName.Contains(str))
                                    select new DocumentViewModel
                                    {
                                        DocumentId = x.DocumentId,
                                        DocumentTitle = x.DocumentTitle,
                                        DocumentName = x.DocumentName,
                                        CategoryId = x.CategoryId,
                                        CategoryName = x.Category.CategoryName
                                    };
                return searcheditems.AsQueryable();
            }

            if (!string.IsNullOrEmpty(cat))
            {
                var searcheditems = from x in doc.Where(x => x.Category.CategoryName.Contains(cat))
                                    select new DocumentViewModel
                                    {
                                        DocumentId = x.DocumentId,
                                        DocumentTitle = x.DocumentTitle,
                                        DocumentName = x.DocumentName,
                                        CategoryId = x.CategoryId,
                                        CategoryName = x.Category.CategoryName
                                    };
                return searcheditems.AsQueryable();
            }

            return items.AsQueryable();
        }

        public bool DocumentUpload(IFormFile file, string path, Document document)
        {
            string path_Root = path;
            string filePath = "\\Documents\\" + file.FileName;
            string extention = Path.GetExtension(file.FileName);
            if (extention == ".pdf" || extention == ".PDF" || extention == ".doc" || extention == ".DOC" || extention == ".docx" || extention == ".DOCX")
            {
                try
                {
                    using (var stream = new FileStream(path_Root + filePath, FileMode.Create))
                    {
                        Document item = new Document();
                        item.DocumentPath = filePath;
                        item.DocumentTitle = document.DocumentTitle;
                        item.DocumentName = file.FileName;
                        item.DocumentPublication = document.DocumentPublication;
                        item.DocumentDescription = document.DocumentDescription;
                        item.CategoryId = document.CategoryId;
                        _context.Add(item);
                        _context.SaveChanges();
                        file.CopyTo(stream);
                    }
                }
                catch (Exception ex)
                {
                    var exp = ex;
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public string getPath(int id)
        {
            var item = _context.Documents.Where(x => x.DocumentId == id).FirstOrDefault();
            return item.DocumentPath;
        }

        public string getName(int id)
        {
            var item = _context.Documents.Where(x => x.DocumentId == id).FirstOrDefault();
            return item.DocumentName;
        }

        public int getDocumentAmount()
        {
            return _context.Documents.Count();
        }

        public DocumentViewModel getDocumentDetails(int id)
        {
            var items = (from x in _context.Documents
                         where x.DocumentId == id
                         select new DocumentViewModel
                         {
                             DocumentId = x.DocumentId,
                             DocumentTitle = x.DocumentTitle,
                             DocumentName = x.DocumentName,
                             CategoryId = x.CategoryId,
                             CategoryName = x.Category.CategoryName
                         }).First();

            documentVM = new DocumentViewModel()
            {
                DocumentName = items.DocumentName,
                DocumentDescription = items.DocumentDescription,
                DocumentPublicationTime = items.DocumentPublicationTime,
                DocumentTitle = items.DocumentTitle,
                CategoryName = items.CategoryName
            };

            return items;
        }
    }
}
