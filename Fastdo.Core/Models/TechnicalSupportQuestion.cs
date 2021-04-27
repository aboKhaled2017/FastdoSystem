using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fastdo.Core.Models
{
    public class TechnicalSupportQuestion
    {
        public TechnicalSupportQuestion()
        {
            CreatedAt = DateTime.Now;
           
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public EUserType UserType { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string  Question { get; set; }
        [DataType(DataType.Text)]
        public string Response { get; set; }
        public DateTime? SeenAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("SenderId")]
        public virtual AppUser User { get; set; }
    }
}
