using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Stocks
{
    public class UpdateStockClassForPharmaModel
    {
        [Required(ErrorMessage ="التصنيف لايمكن ان يكون فارغ")]
        [DataType(DataType.Text)]
        public string NewClass { get; set; }
        [Required(ErrorMessage = "التصنيف لايمكن ان يكون فارغ")]
        [DataType(DataType.Text)]
        public string OldClass { get; set; }
    }
}
