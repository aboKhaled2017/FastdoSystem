using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Fastdo.API.Repositories;
using Fastdo.API.Areas.AdminPanel.Models;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Identity;
using Fastdo.Core;

namespace Fastdo.API.Areas.AdminPanel.Controllers
{
    [Authorize(policy: "AdminPanelAuthPolicy")]
    [Area("AdminPanel")]
    public class MainController : Controller
    {
        protected IUnitOfWork _unitOfWork { get; }
        protected UserManager<AppUser> _userManager { get; }

        public MainController(IUnitOfWork unitOfWork,UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        #region helpers function to Auth
        protected ClaimsPrincipal AdminPrincipals(AdministratorAuthSignModel model, AppUser user)
        {
            var userData = JsonConvert.SerializeObject(model);
            var userClaims = _userManager.GetClaimsAsync(user).Result;
            var claims = new List<Claim> {
                new Claim("UserId",model.Id),
                new Claim (ClaimTypes.NameIdentifier,model.UserName),
                new Claim (ClaimTypes.Name, model.Name),
                new Claim (ClaimTypes.CookiePath, $"/{Variables.AdminPanelCookiePath}"),
                new Claim (ClaimTypes.Role,Variables.adminer),
                new Claim (ClaimTypes.UserData, userData)

            };
            claims.AddRange(userClaims);
            var identity = new ClaimsIdentity(claims, Variables.AdminSchemaOfAdminSite);
            var principals = new ClaimsPrincipal(identity);
            return principals;
        }
        protected Task _SignAdminInAsync(AdministratorAuthSignModel model)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var probs = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1)
            };
            /*await Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions
            .SignInAsync(HttpContext, "StudentScheme", StudentPrincipals(email, name, id), probs);*/
            LogoutAdmin().Wait();
            var user = _userManager.FindByIdAsync(model.Id).Result;
            return HttpContext.SignInAsync(Variables.AdminSchemaOfAdminSite, AdminPrincipals(model,user), probs);
        }
        protected async Task LogoutAdmin()
        {

            await HttpContext.SignOutAsync(Variables.AdminSchemaOfAdminSite);
            
            HttpContext.Response.Cookies.Append("ASP.NET_SessionId","");
        }
        #endregion
    }
}