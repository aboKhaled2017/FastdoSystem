using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace System_Back_End.Models
{
    public class PhrDrgRequestAddModel
    {
        [Required]
        public string PharmacyId { get; set; }
        [Required]
        public Guid LzDrugId { get; set; }
    }
}
