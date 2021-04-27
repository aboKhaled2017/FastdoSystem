using Fastdo.Core.Utilities;
using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace Fastdo.Core.ViewModels
{
    public class Phr_RegisterModel_Proof : IPhr_RegisterModel_Proof
    {
        [Required(ErrorMessage = "الصورة مطلوبة")]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { "png", "jpg", "jpeg", "gif", "PNG", "JPG", "GIF", "JPEG" })]
        public IFormFile CommerialRegImg { get; set; }

        [Required(ErrorMessage = "الصورة مطلوبة")]
        [AllowedExtensions(new string[] { "png", "jpg", "jpeg", "gif", "PNG", "JPG", "GIF", "JPEG" })]
        [DataType(DataType.Upload)]
        public IFormFile LicenseImg { get; set; }
    }
}
