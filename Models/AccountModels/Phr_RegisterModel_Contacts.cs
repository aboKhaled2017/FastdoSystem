using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System_Back_End.Utilities;

namespace System_Back_End.Models
{
    [ModelMetadataType(typeof(IPhr_RegisterModel_Contacts))]
    public class Phr_RegisterModel_Contacts : IPhr_RegisterModel_Contacts
    {
        public string PresPhone { get;set; }
        public string LinePhone {get;set; }
        public string Address {get;set; }
    }
}
