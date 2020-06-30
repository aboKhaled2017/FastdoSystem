using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace System_Back_End.Models
{
    [ModelMetadataType(typeof(IPhr_RegisterModel_Identity))]
    public class Phr_RegisterModel_Identity : IPhr_RegisterModel_Identity
    {
        public string Name { get;set; }
        public string mgrName { get;set; }
        public string ownerName { get;set; }
        public byte CityId { get ; set; }
        public byte AreaId { get ; set; }
    }
}
