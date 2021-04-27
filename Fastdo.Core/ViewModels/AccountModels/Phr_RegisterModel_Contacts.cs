
using System.ComponentModel.DataAnnotations;
using Fastdo.Core.Utilities;
using Fastdo.Core.Enums;

namespace Fastdo.Core.ViewModels
{

    public class Phr_RegisterModel_Contacts 
    {
        
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression("^((010)|(011)|(012)|(015)|(017))[0-9]{8}$", ErrorMessage = "رقم هاتف غير صالح")]
        [CheckIfUserPropValueIsExixts("PersPhone", UserPropertyType.phone)]
        public string PersPhone { get; set; }
        [Required(ErrorMessage = "رقم التليفون الارضى مطلوب")]
        [RegularExpression("^[0-9]{4,}$", ErrorMessage = "رقم تليفون غير صحيح")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "رقم تليفون من 4 الى 15 رقم")]
        public string LinePhone { get; set; }
        public string Address { get; set; }
    }
}
