using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    public class LzDrgRequestAddModel
    {
        [Required]
        public string PharmacyId { get; set; }
        [Required]
        public Guid LzDrugId { get; set; }
    }
}
