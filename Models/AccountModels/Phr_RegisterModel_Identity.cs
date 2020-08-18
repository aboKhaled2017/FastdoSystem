using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fastdo.backendsys.Models
{
    [ModelMetadataType(typeof(IPhr_RegisterModel_Identity))]
    public class Phr_RegisterModel_Identity : IPhr_RegisterModel_Identity
    {
        public string Name { get;set; }
        public string MgrName { get;set; }
        public string OwnerName { get;set; }
        public byte CityId { get ; set; }
        public byte AreaId { get ; set; }
    }
}
