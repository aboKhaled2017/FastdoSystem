﻿using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System_Back_End.Services;

namespace System_Back_End.Utilities
{
    [Authorize]
    public class CheckIfUserPropValueIsExixtsOnUpdate:ValidationAttribute
    {
        private readonly string _propertyNameToBeChecked;
        private readonly UserPropertyType _userPropertyType;
        private readonly System.Models.SysDbContext _Context;
        public CheckIfUserPropValueIsExixtsOnUpdate(string propertyNameToBeChecked,UserPropertyType userPropertyType)
        {
            _propertyNameToBeChecked = propertyNameToBeChecked;
            _userPropertyType = userPropertyType;
            _Context = RequestStaticServices.GetDbContext();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var checkedProperty = validationContext.ObjectType.GetProperty(_propertyNameToBeChecked);
            if(checkedProperty == null)
                return new ValidationResult($"some Property '{_propertyNameToBeChecked}' is undefined.");
            var propertyValue = checkedProperty.GetValue(validationContext.ObjectInstance, null);
            if(propertyValue==null)
                return new ValidationResult($"the value of {_propertyNameToBeChecked} property is null");
            string valueStr = propertyValue.ToString();
            switch (_userPropertyType)
            {
                case UserPropertyType.email:
                    {
                        if (valueStr.Trim() == Functions.UserIdentifier().Email)
                            return new ValidationResult("انت لم تقم باى تغيير");
                        if (_Context.Users.Any(u=>u.Email== valueStr))
                            return new ValidationResult($"البريد الالكترونى {valueStr} بالفعل محجوز");
                    };
                    break;
                case UserPropertyType.phone:
                    {
                        if (valueStr.Trim() == Functions.UserIdentifier().Phone)
                            return new ValidationResult("انت لم تقم باى تغيير");
                        if (_Context.Users.Any(u => u.PhoneNumber == valueStr))
                            return new ValidationResult($"رقم الهاتق {valueStr} بالفعل محجوز");
                    };
                    break;
                case UserPropertyType.userName:
                    {
                        if (valueStr.Trim() == Functions.UserIdentifier().UserName)
                            return new ValidationResult("انت لم تقم باى تغيير");
                        if (_Context.Users.Any(u => u.UserName == valueStr))
                            return new ValidationResult($"اسم المستخدم {valueStr} بالفعل محجوز");
                    };
                    break;
                default:
                   return new ValidationResult($"not valid property type"); 
            }
            return ValidationResult.Success;
        }
    }
}