using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.ViewModels
{
    public class DocumentViewModel
    {
        public int DocumentId { get; set; }
        [Display(Name = "Title")]
        public string DocumentTitle { get; set; }
        public string DocumentFormat { get; set; }
        [Display(Name = "Name")]
        public string DocumentName { get; set; }
        [Display(Name = "Description")]
        public string DocumentDescription { get; set; }
        [Display(Name = "Publication Date")]
        public DateTime DocumentPublicationTime { get; set; }
        public int? CategoryId { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
    }
}
