using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fastdo.Core.Enums;
using System.Text;

namespace Fastdo.Core.Models
{
    public class StkDrugInStkDrgPackageReq
    {
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid StkDrugId { get; set; }
        [Required]      
        public Guid StkPackageId { get; set; }
        [Required]
        public string StockId { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int Quantity { get; set; }

        [ForeignKey("StkDrugId")]
        [InverseProperty("RequestedPackages")]
        public virtual StkDrug StkDrug { get; set; }

        [ForeignKey("StkPackageId")]
        [InverseProperty("AssignedStkDrugs")]
        public virtual StockInStkDrgPackageReq StockPackage { get; set; }
    }
}
