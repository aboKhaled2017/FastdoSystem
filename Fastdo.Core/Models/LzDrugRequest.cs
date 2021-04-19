using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fastdo.Core.Enums;
using System.Text;

namespace Fastdo.Core.Models
{
    public class LzDrugRequest
    {
        public LzDrugRequest()
        {
            Status = LzDrugRequestStatus.Pending;
            Seen = false;
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string PharmacyId { get; set; }
        [Required]
        public Guid LzDrugId { get; set; }
        public LzDrugRequestStatus Status { get; set; }
        public int Quantity { get; set; }
        public bool Seen { get; set; }
        [ForeignKey("PharmacyId")]
        [InverseProperty("RequestedLzDrugs")]
        public virtual Pharmacy Pharmacy { get; set; }
        [ForeignKey("LzDrugId")]
        [InverseProperty("RequestingPharms")]        
        public virtual LzDrug LzDrug { get; set; }
    }
}
