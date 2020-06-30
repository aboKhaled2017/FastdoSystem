using System.ComponentModel.DataAnnotations;
using System_Back_End.Utilities;

namespace System_Back_End.Models
{
    public interface IPhr_RegisterModel_Contacts
    {
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression("^((010)|(011)|(012)|(015)|(017))[0-9]{8}$",ErrorMessage = "رقم هاتف غير صالح")]
        [CheckIfUserPropValueIsExixts("PresPhone", UserPropertyType.phone)]
        string PresPhone { get; set; }
        [Required(ErrorMessage = "رقم التليفون الارضى مطلوب")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "رقم تليفون غير صالح")]
        string LinePhone { get; set; }
        string Address { get; set; }
    }
}
