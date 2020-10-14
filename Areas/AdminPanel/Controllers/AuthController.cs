using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Areas.AdminPanel.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Fastdo.backendsys.Areas.AdminPanel.Controllers
{
    public class AuthController : MainController
    {
        public AuthController(IAdminRepository adminRepository, UserManager<AppUser> userManager) : base(adminRepository, userManager)
        {
        }

        [AllowAnonymous]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult SignIn(string returnUrl = "")
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignIn(AdministratorAuthSignModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            ViewData["ReturnUrl"] = model.ReturnUrl;
            var result =_SignAdminInAsync(model);
            if (result.IsCompletedSuccessfully)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult LogOut()
        {
            LogoutAdmin().Wait();
            return RedirectPermanent("/AdminPanel/Auth/SignIn");
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}