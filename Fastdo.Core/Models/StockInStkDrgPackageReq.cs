using Fastdo.Core.Enums;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fastdo.Core.Models
{
    public class StockInStkDrgPackageReq
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string StockId { get; set; }
        [Required]
        public Guid PackageId { get; set; }
        public StkDrugPackageRequestStatus Status { get; set; } = StkDrugPackageRequestStatus.Pending;
        public bool Seen { get; set; } = false;

        [ForeignKey("PackageId")]
        [InverseProperty("AssignedStocks")]
        public virtual StkDrugPackageRequest Package { get; set; }

        [ForeignKey("StockId")]
        [InverseProperty("RequestedPackages")]
        public virtual Stock Stock { get; set; }
        [InverseProperty("StockPackage")]
        public virtual ICollection<StkDrugInStkDrgPackageReq> AssignedStkDrugs { get; set; }
    }
}
