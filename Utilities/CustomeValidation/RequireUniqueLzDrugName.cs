using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fastdo.backendsys.Services;

namespace Fastdo.backendsys.Utilities
{
    public class RequireUniqueLzDrugName : ValidationAttribute
    {
        private readonly Fastdo.Core.Models.SysDbContext _Context;

        public bool _isUpdate { get; }

        public RequireUniqueLzDrugName(bool isUpdate=false)
        {
            _Context = RequestStaticServices.GetDbContext();
            _isUpdate = isUpdate;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (_isUpdate)
            {
                var oldNameProperty = validationContext.ObjectType.GetProperty("OldName");
                if (oldNameProperty == null)
                    return new ValidationResult("some Property OldName is undefined.",new List<string> {"Name"});
                var propertyValue = oldNameProperty.GetValue(validationContext.ObjectInstance, null);
                if (propertyValue == null)
                    return new ValidationResult($"the value of OldName property is null", new List<string> { "Name" });
                if (value.ToString().Equals(propertyValue.ToString()))
                    return ValidationResult.Success;
            }
            string valueStr = value.ToString().ToLower();
            string userId = Functions.GetUserId();
            if(_Context.LzDrugs.AnyAsync(d=>d.PharmacyId==userId&& d.Name.ToLower()==valueStr).Result)                
                   return new ValidationResult(ErrorMessage??"لقد قمت بأضافة اسم هذا الراكد من قبل ,يمكنك تعديله", new List<string> { "Name" }); 
           
            return ValidationResult.Success;
        }
    }
}
