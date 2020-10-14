using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Services;
using Fastdo.Core.Models;
namespace Fastdo.backendsys.Utilities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateAreaId:ValidationAttribute
    {
        private SysDbContext _context { get;}
        public ValidateAreaId()
        {
            _context = RequestStaticServices.GetDbContext();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            byte areaId = 0;
            if (!byte.TryParse(value.ToString(),out areaId)){
                return new ValidationResult(GetErrorMessage());
            }
            var cityIdProperty = validationContext.ObjectType.GetProperty("CityId");
            if (cityIdProperty == null)
                return new ValidationResult(GetErrorMessage());
            var propertyValue = cityIdProperty.GetValue(validationContext.ObjectInstance, null);
            if (propertyValue == null)
                return new ValidationResult(GetErrorMessage());
            byte cityId = byte.Parse(propertyValue.ToString());
            if(_context.Areas.Any(a=>a.Id==areaId && a.SuperAreaId==cityId))
                return ValidationResult.Success;
            return new ValidationResult(GetErrorMessage());
        }
        private string GetErrorMessage()
        {
            return $"هذه المنطقة غير موجودة";
        }
    }
}
