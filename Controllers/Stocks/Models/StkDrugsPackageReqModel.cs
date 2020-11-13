using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Stocks
{
    public class StkDrugsPackageReqModel_PharmaData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string AddressInDetails { get; set; }
        public string PhoneNumber { get; set; }
        public string LandLinePhone { get; set; }
    }
    public class StkDrugsPackageReqModel_DrugData
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
    }
    public class StkDrugsPackageReqModel
    {
        public Guid PackageId { get; set; }
        public Guid StkPackageId { get; set; }

        public StkDrugsPackageReqModel_PharmaData Pharma { get; set; }
        public IEnumerable<StkDrugsPackageReqModel_DrugData> Drugs { get; set; }
        public StkDrugPackageRequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
