using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System_Back_End.Utilities;

namespace System_Back_End.Models
{
    public class PharmacyClientRegisterModel :
        IPhr_RegisterModel_Identity,
        IPhr_RegisterModel_Proof,
        IPhr_RegisterModel_Contacts,
        IPhr_RegisterModel_Account
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "الاسم مابين 5 الى 20 حرف")]
        public string Name { get; set; }
        [Required(ErrorMessage = "الاسم مطلوب")]
        [MaxLength(50, ErrorMessage = "الاسم لا يجب ان يتعدى 60 حرف")]
        [RegularExpression("^[a-zA-Z]{3,15}(?: [a-zA-Z]+){1,3}$", ErrorMessage = "ادخل اسم صحيح, وان يكون ثنائى على الاقل")]
        public string mgrName { get; set; }
        [Required(ErrorMessage = "الاسم مطلوب")]
        [MaxLength(50, ErrorMessage = "الاسم لا يجب ان يتعدى 60 حرف")]
        [RegularExpression("^[a-zA-Z]{3,15}(?: [a-zA-Z]+){1,3}$", ErrorMessage = "ادخل اسم صحيح, وان يكون ثنائى على الاقل")]
        public string ownerName { get; set; }

        [Required(ErrorMessage = "الصورة مطلوبة")]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { "png", "jpg", "jpeg", "gif", "PNG", "JPG", "GIF", "JPEG" })]
        public IFormFile CommerialRegImg { get; set; }

        [Required(ErrorMessage = "الصورة مطلوبة")]
        [AllowedExtensions(new string[] { "png", "jpg", "jpeg", "gif", "PNG", "JPG", "GIF", "JPEG" })]
        [DataType(DataType.Upload)]
        public IFormFile LicenseImg { get; set; }
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression("^((010)|(011)|(012)|(015)|(017))[0-9]{8}$", ErrorMessage = "رقم هاتف غير صالح")]
        [CheckIfUserPropValueIsExixts("PresPhone", UserPropertyType.phone)]
        public string PresPhone { get; set; }
        [Required(ErrorMessage = "رقم التليفون الارضى مطلوب")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "رقم تليفون غير صالح")]
        public string LinePhone { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "البريد الالكترونى مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الالكترونى غير صحيح")]
        [CheckIfUserPropValueIsExixts("Email", UserPropertyType.email)]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة السر مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة السر على الاقل {1} من الرموز وعلى الاكثر {2} من الرموز", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة السر وتأكيدها غير متطابقتان")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "من فضلك اختر المدينة")]
        [Range(1, 256, ErrorMessage = "من فضلك اختر المدينة")]
        public byte CityId { get; set; }
        [Range(1, 256, ErrorMessage = "من فضلك اختر المركز")]
        [Required(ErrorMessage = "من فضلك اختر المركز")]
        [ValidateAreaId]
        public byte AreaId { get; set; }
    }
}
