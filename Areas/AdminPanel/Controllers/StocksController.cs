﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.backendsys.Repositories;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fastdo.backendsys.Areas.AdminPanel.Controllers
{
    [Authorize(Policy = "ControlOnStocksPagePolicy")]
    public class StocksController : MainController
    {
        public StocksController(IAdminRepository adminRepository, UserManager<AppUser> userManager) : base(adminRepository, userManager)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}