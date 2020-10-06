using Fastdo.backendsys.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fastdo.backendsys.Controllers.Stocks
{
    public class StockDrugsReporstModel
    {

        public int ColNameOrder { get; set; } = 0;
        
        public int ColPriceOrder { get; set; } = 1;

        public int ColDiscountOrder { get; set; } = 2;
        [Required(ErrorMessage ="من فضلك اختر التصنيف")]
        public Guid ForClassId { get; set; }

        [Required(ErrorMessage ="من فضلك ادخل  ملف الاكسل الخاص بالبيانات")]
        [DataType(DataType.Upload,ErrorMessage ="هذه البيانات لا تمثل ملف")]
        [AllowedExtensions(new string[] { "xlsm", "xlsx", "xlsb", "xls" })]
        public IFormFile Sheet { get; set; }
    }
}