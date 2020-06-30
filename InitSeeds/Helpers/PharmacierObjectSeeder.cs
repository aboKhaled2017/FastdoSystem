using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Models;
namespace System_Back_End.InitSeeds.Helpers
{
    public class PharmacierObjectSeeder:Pharmacy
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
