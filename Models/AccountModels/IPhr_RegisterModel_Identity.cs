using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Fastdo.backendsys.Utilities;

namespace Fastdo.backendsys.Models
{
    public interface IPhr_RegisterModel_Identity
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "الاسم مابين 5 الى 20 حرف")]
        string Name { get; set; }
        [Required(ErrorMessage = "الاسم مطلوب")]
        [MaxLength(50, ErrorMessage = "الاسم لا يجب ان يتعدى 60 حرف")]
        [RegularExpression("^([a-zA-Z]|[\u0600-\u06FF ]){3,15}(?: ([a-zA-Z]|[\u0600-\u06FF ])+){1,3}$", ErrorMessage = "ادخل اسم صحيح, وان يكون ثنائى على الاقل")]
        string MgrName { get; set; }
        [Required(ErrorMessage = "الاسم مطلوب")]
        [MaxLength(50, ErrorMessage = "الاسم لا يجب ان يتعدى 60 حرف")]
        [RegularExpression("^([a-zA-Z]|[\u0600-\u06FF ]){3,15}(?: ([a-zA-Z]|[\u0600-\u06FF ])+){1,3}$", ErrorMessage = "ادخل اسم صحيح, وان يكون ثنائى على الاقل")]
        string OwnerName { get; set; } 
        [Required(ErrorMessage = "من فضلك اختر المدينة")]
        [Range(1,256,ErrorMessage = "من فضلك اختر المدينة")]
        byte CityId { get; set; }
        [Range(1,256,ErrorMessage = "من فضلك اختر المركز")]
        [Required(ErrorMessage ="من فضلك اختر المركز")]
        [ValidateAreaId]
        byte AreaId { get; set; }
    }
}
