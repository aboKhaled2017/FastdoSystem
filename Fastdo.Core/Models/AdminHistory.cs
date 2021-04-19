using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fastdo.Core.Models
{
    public class AdminHistory
    {
        public AdminHistory()
        {
            OccuredAt = DateTime.Now;
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime OccuredAt { get; set; }
        public string IssuerId { get; set; }
        public string OperationType { get; set; }
        public string ToId { get; set; }
        public string Desc { get; set; }
    }
}
