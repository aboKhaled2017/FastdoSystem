﻿
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System_Back_End.Services;

namespace System_Back_End
{
    public static class Functions
    {
        public static string getRoleOfUserType(UserType userType)
        {
            return userType == UserType.pharmacier
                ? Variables.pharmacier
                : userType == UserType.stocker
                ? Variables.stocker
                : "adminer";
        }
        public static string GetRandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
        public static string GenerateConfirmationTokenCode()
        {
            return GetRandomDigits(15);
        }
        public static ErrorsResult MakeError(string key,string error)
        {
            var dynamicObject = new ExpandoObject() as IDictionary<string, object>;
            dynamicObject.Add(key, error);

            return new ErrorsResult
            {
                errors = dynamicObject
            };
        }
        public static ErrorsResult MakeError(string error)
        {
            return MakeError("G", error);
        }
        public static string GetUsersImagesPath____(HttpContext httpContext)
        {
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/Users/";
        }
        public static UserIdentifier UserIdentifier()
        {
            var User = RequestStaticServices.GetCurrentHttpContext().User;
                return new UserIdentifier
                {
                    Email = User.Claims.FirstOrDefault(c => c.Type == "Email").Value,
                    Name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
                    UserId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value,
                    Phone = User.Claims.FirstOrDefault(c => c.Type == "Phone").Value,
                    UserName = User.Claims.FirstOrDefault(c => c.Type == "UserName").Value,
                    IsEmailConfirmed=bool.Parse(User.Claims.FirstOrDefault(c => c.Type == "IsEmailConfirmed").Value),
                    Role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value,
                };            
        }
        public static UserType CurrentUserType()
        {
            var User = RequestStaticServices.GetCurrentHttpContext().User;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                return role == Variables.pharmacier
                    ? UserType.pharmacier
                    : UserType.stocker;
        }
    }
}
