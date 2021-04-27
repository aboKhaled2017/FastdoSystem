using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.API.Areas.AdminPanel.Models;
using Fastdo.API.Repositories;
using Fastdo.Core;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.API.Areas.AdminPanel.Controllers
{
    public class HomeController : MainController
    {
        public HomeController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager) : base(unitOfWork, userManager)
        {
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.AdminRepository.GetGeneralStatisOfSystem());
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