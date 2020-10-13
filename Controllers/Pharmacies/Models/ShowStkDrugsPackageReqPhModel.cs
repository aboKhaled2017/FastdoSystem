using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Pharmacies
{
    public class ShowStkDrugsPackageReqPhModel
    {
        [Required(ErrorMessage ="من فضلك ادخل اسم الطلبية")]
        [StringLength(60,MinimumLength =3)]
        public string Name { get; set; }
        public IEnumerable<ShowStkDrugsPackageReqPh_fromStockModel> FromStocks { get; set; }
    }
    public class ShowStkDrugsPackageReqPh_fromStockModel
    {
        public string StockId { get; set; }
        public IEnumerable<IEnumerable<dynamic>> DrugsList { get; set; }
    }

}
