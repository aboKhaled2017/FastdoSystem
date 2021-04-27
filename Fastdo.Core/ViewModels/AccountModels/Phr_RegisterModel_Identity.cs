
using Fastdo.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Fastdo.Core.ViewModels
{
    public class Phr_RegisterModel_Identity : IPhr_RegisterModel_Identity
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "الاسم مابين 5 الى 20 حرف")]
        public string Name { get; set; }
        [Required(ErrorMessage = "الاسم مطلوب")]
        [MaxLength(50, ErrorMessage = "الاسم لا يجب ان يتعدى 60 حرف")]
        [RegularExpression("^([a-zA-Z]|[\u0600-\u06FF ]){3,15}(?: ([a-zA-Z]|[\u0600-\u06FF ])+){1,3}$", ErrorMessage = "ادخل اسم صحيح, وان يكون ثنائى على الاقل")]
        public string MgrName { get; set; }
        [Required(ErrorMessage = "الاسم مطلوب")]
        [MaxLength(50, ErrorMessage = "الاسم لا يجب ان يتعدى 60 حرف")]
        [RegularExpression("^([a-zA-Z]|[\u0600-\u06FF ]){3,15}(?: ([a-zA-Z]|[\u0600-\u06FF ])+){1,3}$", ErrorMessage = "ادخل اسم صحيح, وان يكون ثنائى على الاقل")]
        public string OwnerName { get; set; }
        [Required(ErrorMessage = "من فضلك اختر المدينة")]
        [Range(1, 256, ErrorMessage = "من فضلك اختر المدينة")]
        public byte CityId { get; set; }
        [Range(1, 256, ErrorMessage = "من فضلك اختر المركز")]
        [Required(ErrorMessage = "من فضلك اختر المركز")]
        [ValidateAreaId]
        public byte AreaId { get; set; }
    }
}
