using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Fastdo.backendsys.Utilities;

namespace Fastdo.backendsys.Models
{
    [ModelMetadataType(typeof(IPhr_RegisterModel_Contacts))]
    public class Phr_RegisterModel_Contacts : IPhr_RegisterModel_Contacts
    {
        public string PersPhone { get;set; }
        public string LinePhone {get;set; }
        public string Address {get;set; }
    }
}
