using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fastdo.backendsys.Models
{
    [ModelMetadataType(typeof(IPhr_RegisterModel_Proof))]
    public class Phr_RegisterModel_Proof : IPhr_RegisterModel_Proof
    {
        public IFormFile CommerialRegImg { get; set; }
        public IFormFile LicenseImg { get; set; }
    }
}
