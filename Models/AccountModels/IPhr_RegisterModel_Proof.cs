using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Fastdo.backendsys.Utilities;

namespace Fastdo.backendsys.Models
{
    public interface IPhr_RegisterModel_Proof
    {
        [Required(ErrorMessage = "الصورة مطلوبة")]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { "png", "jpg", "jpeg", "gif", "PNG", "JPG", "GIF", "JPEG" })]
        IFormFile CommerialRegImg { get; set; }

        [Required(ErrorMessage = "الصورة مطلوبة")]
        [AllowedExtensions(new string[] { "png", "jpg", "jpeg", "gif", "PNG", "JPG", "GIF", "JPEG" })]
        [DataType(DataType.Upload)]
        IFormFile LicenseImg { get; set; }
    }
}
