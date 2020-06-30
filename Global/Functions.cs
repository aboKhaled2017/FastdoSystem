
using System;

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
    }
}
