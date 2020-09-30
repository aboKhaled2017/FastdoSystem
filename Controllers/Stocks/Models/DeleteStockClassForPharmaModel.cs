using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Stocks
{
    public class DeleteStockClassForPharmaModel
    {
        [Required(ErrorMessage = "ادخل قيمة")]
        public string DeletedClass { get; set; }
        public string ReplaceClass { get; set; }
    }
}
