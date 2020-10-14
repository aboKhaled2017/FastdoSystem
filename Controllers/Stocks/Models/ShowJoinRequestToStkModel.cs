using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Stocks
{
    public class ShowJoinRequestToStk_pharmaDataModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string AddressInDetails { get; set; }
        public string PhoneNumber { get; set; }
        public string LandlinePhone { get; set; }
    }
    public class ShowJoinRequestToStkModel
    {
        public ShowJoinRequestToStk_pharmaDataModel Pharma { get; set; }                       
        public PharmacyRequestStatus Status { get; set; }
        public string PharmaClass { get; set; }
        public bool Seen { get; set; }
    }
    public class ShowJoinedPharmaToStkModel
    {
        public ShowJoinRequestToStk_pharmaDataModel Pharma { get; set; }
        public Guid PharmaClassId { get; set; }
        public PharmacyRequestStatus Status { get; set; }

    }
}
