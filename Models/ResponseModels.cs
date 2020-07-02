using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System_Back_End.Models
{
    public class GeneralResponseModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PersPhone { get; set; }
        public string LandlinePhone { get; set; }
        public bool EmailConfirmed { get; set; }
    }
    public class PharmacyClientResponseModel:GeneralResponseModel
    {
        
    }
    public class StockClientResponseModel : GeneralResponseModel
    {
         
    }
}
