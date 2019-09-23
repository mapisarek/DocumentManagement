using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        [Required]
        public string DocumentTitle { get; set; }
        [Required]
        public string DocumentPath { get; set; }
        [Required]
        public string DocumentName { get; set; }
        [Required]
        public string DocumentDescription { get; set; }
        [Required]
        public DateTime DocumentPublication { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
