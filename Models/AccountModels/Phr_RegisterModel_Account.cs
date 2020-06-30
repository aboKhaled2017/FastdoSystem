using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System_Back_End.Utilities;

namespace System_Back_End.Models
{
    [ModelMetadataType(typeof(IPhr_RegisterModel_Account))]
    public class Phr_RegisterModel_Account : IPhr_RegisterModel_Account
    {
        public string Email { get;set; }
        public string Password { get;set; }
        public string ConfirmPassword { get;set; }
    }
}
