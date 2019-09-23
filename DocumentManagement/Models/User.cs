using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DocumentManagement.Models
{
    public class User
    {
        public static ClaimsIdentity Identity { get; set; }
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserRole { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
