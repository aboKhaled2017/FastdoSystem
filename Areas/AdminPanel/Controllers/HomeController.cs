using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Areas.AdminPanel.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Areas.AdminPanel.Controllers
{
    public class HomeController : MainController
    {
        public HomeController(IAdminRepository adminRepository, UserManager<AppUser> userManager) : base(adminRepository, userManager)
        {
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            return View(await _adminRepository.GetGeneralStatisOfSystem());
        }
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            
            return View(new ErrorViewModel {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode=HttpContext.Response.StatusCode
            });
        }
    }
}