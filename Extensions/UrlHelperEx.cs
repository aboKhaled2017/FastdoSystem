using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Controllers.Auth;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperEx
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AuthController.ConfirmEmailForUser),
                controller: "Auth",
                values: new { userId, code },
                protocol: scheme);
        }
        public static string ChangeEmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(ManageController.ChangeEmailForUser),
                controller: "manage",
                values: new { userId, code },
                protocol: scheme);
        }
        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AuthController.ResetPasswordForUser),
                controller: "Auth",
                values: new { userId, code },
                protocol: scheme);
        }
        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code,string newPassword, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AuthController.ResetPasswordForUser),
                controller: "Auth",
                values: new { userId, code ,newPassword},
                protocol: scheme);
        }
    }
}
