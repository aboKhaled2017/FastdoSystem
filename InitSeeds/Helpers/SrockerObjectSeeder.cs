using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.Core.Models;
namespace Fastdo.backendsys.InitSeeds.Helpers
{
    public class StockerObjectSeeder:Stock
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
