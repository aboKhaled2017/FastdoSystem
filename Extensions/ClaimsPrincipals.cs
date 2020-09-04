using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    public static class ClaimsPrincipals
    {
        public static T GetUserData<T>(this ClaimsPrincipal User)
        {
            if (User == null)
                return default(T);
            return JsonConvert.DeserializeObject<T>(User.Claims.Single(c => c.Type == ClaimTypes.UserData).Value);
        }
    }
}
