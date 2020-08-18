using System.ComponentModel.DataAnnotations;
using Fastdo.backendsys.Utilities;

namespace Fastdo.backendsys.Models
{
    public interface IPhr_RegisterModel_Contacts
    {
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression("^((010)|(011)|(012)|(015)|(017))[0-9]{8}$",ErrorMessage = "رقم هاتف غير صالح")]
        [CheckIfUserPropValueIsExixts("PersPhone", UserPropertyType.phone)]
        string PersPhone { get; set; }
        [Required(ErrorMessage = "رقم التليفون الارضى مطلوب")]
        [RegularExpression("^[0-9]{4,}$",ErrorMessage ="رقم تليفون غير صحيح")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "رقم تليفون من 4 الى 15 رقم")]
        string LinePhone { get; set; }
        string Address { get; set; }
    }
}
