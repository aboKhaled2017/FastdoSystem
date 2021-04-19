using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Fastdo.Core.Models
{
    public partial class Admin
    {
        public Admin()
        {
            SuperAdminId = null;
        }
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }

        [ForeignKey("Id")]
        public virtual AppUser User { get; set; }
        public string SuperAdminId { get; set; } 
        [ForeignKey("SuperAdminId")]
        public virtual Admin SuperAdmin { get; set; }
        public virtual ICollection<Admin> SubAdmins { get; set; }

    }
}