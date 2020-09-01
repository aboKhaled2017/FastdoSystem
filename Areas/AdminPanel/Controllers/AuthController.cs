using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Areas.AdminPanel.Controllers
{
    public class AuthController : MainController
    {
        [AllowAnonymous]
        public async Task<IActionResult> SignIn()
        {
            return View();
        }
    }
}